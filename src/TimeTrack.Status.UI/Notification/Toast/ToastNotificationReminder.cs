using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TimeTrack.Application.Common;
using TimeTrack.Application.Common.Interfaces;
using TimeTrack.Interfaces;
using TimeTrack.Status.UI.Notification.Toast.Configuration;
using Windows.UI.Notifications;

namespace TimeTrack.Status.UI.Notification.Toast
{
    public class ToastNotificationReminder : IReminder
    {
        private readonly IConfiguration _configuration;
        private readonly IDataStorage _dataStorage;
        private readonly ITextTemplateRenderer _textTemplateRenderer;
        private readonly ITimeServiceProvider _timeServiceProvider;
        private string _applicationId = "TimeTrack.Status.UI.ReminderNotification";

        private Dictionary<string, DateTime> _notificationLogbook = new Dictionary<string, DateTime>();

        public ToastNotificationReminder(IDataStorage dataStorage, ITimeServiceProvider timeServiceProvider, IConfiguration configuration, ITextTemplateRenderer textTemplateRenderer)
        {
            _dataStorage = dataStorage;
            _timeServiceProvider = timeServiceProvider;
            _configuration = configuration;
            this._textTemplateRenderer = textTemplateRenderer;
        }

        public bool DisableIdleReminder { get; set; }
        public bool DisableTrackingReminder { get; set; }

        public bool NotificationsActive
        {
            get
            {
                var configuration = GetReminderConfiguration();

                if (DisableIdleReminder && DisableTrackingReminder)
                {
                    return false;
                }

                return !IsSilentTime(configuration);
            }
        }

        public void OnTrackingStatusChanged()
        {
            _notificationLogbook.Clear();
        }

        public async Task TestNotificationAsync()
        {
            var configuration = GetReminderConfiguration();

            await ExecuteTestNotification(configuration);
        }

        public async Task UpdateAsync()
        {
            var configuration = GetReminderConfiguration();

            if (DisableIdleReminder && DisableTrackingReminder)
            {
                return;
            }

            if (IsSilentTime(configuration))
            {
                return;
            }

            await ExecuteAsync(configuration);
        }

        private static async Task ShowToastNotification(string applicationId, ToastNotification toast)
        {
            var manager = new SynchronousToastNotificationManager(toast);
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(30));
                await manager.ShowAndWaitAsync(applicationId, cancellationTokenSource.Token, true);
            }
        }

        private async Task CheckReminderRulesForNonTracking(Data data, ReminderConfiguration configuration)
        {
            if (configuration.OnIdle == null || string.IsNullOrWhiteSpace(configuration.OnIdle.DefinitionName))
            {
                return;
            }

            var notificationThreshold = configuration.OnIdle.NotificationThreshhold;
            if (notificationThreshold > TimeSpan.Zero && data.Duration > notificationThreshold && MayRetrigger("__IDLE__", configuration.OnIdle.RetriggerThreshhold))
            {
                NotificationTriggered("__IDLE__");
                await ShowNotificationByDefinitionName(configuration.OnIdle.DefinitionName, configuration);
            }
        }

        private async Task CheckReminderRulesForTracking(Data data, ReminderConfiguration configuration)
        {
            if (configuration.OnTracking == null)
            {
                return;
            }

            var rule = configuration.OnTracking.FirstOrDefault(x => IsMatchingTrackingRule(x, data));
            if (rule != null && !string.IsNullOrWhiteSpace(rule.DefinitionName) && rule.NotificationThreshhold > TimeSpan.Zero && data.Duration > rule.NotificationThreshhold && MayRetrigger(data.CurrentTrackingActivity.Identifier, rule.RetriggerThreshhold))
            {
                NotificationTriggered(data.CurrentTrackingActivity.Identifier);
                await ShowNotificationByDefinitionName(rule.DefinitionName, configuration);
            }
        }

        private ToastNotification CreateToastFromDefinition(ToastNotificationDefinition toastNotificationDefinition)
        {
            var finalDefinition = RenderNotificationDefinitionTextLines(toastNotificationDefinition);
            if (!string.IsNullOrWhiteSpace(finalDefinition.ImageFileOrUrl))
            {
                finalDefinition.ImageFileOrUrl = Environment.ExpandEnvironmentVariables(finalDefinition.ImageFileOrUrl);
                if (!string.IsNullOrWhiteSpace(finalDefinition.ImageFileOrUrl) && !Path.IsPathRooted(finalDefinition.ImageFileOrUrl) && !finalDefinition.ImageFileOrUrl.Contains("://"))
                {
                    var appBasePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    finalDefinition.ImageFileOrUrl = Path.GetFullPath(Path.Combine(appBasePath, finalDefinition.ImageFileOrUrl));
                }
            }

            return ToastNotificationFactory
                .CreateToastNotificationBuilderFromDefinition(finalDefinition)
                .BuildToastNotification();
        }

        private async Task ExecuteAsync(ReminderConfiguration configuration)
        {
            var data = GetData();
            if (data.IsTracking)
            {
                if (!DisableTrackingReminder)
                {
                    await CheckReminderRulesForTracking(data, configuration);
                }
            }
            else
            {
                if (!DisableIdleReminder)
                {
                    await CheckReminderRulesForNonTracking(data, configuration);
                }
            }
        }

        private async Task ExecuteTestNotification(ReminderConfiguration configuration)
        {
            if (!string.IsNullOrWhiteSpace(configuration?.OnTest))
            {
                await ShowNotificationByDefinitionName(configuration.OnTest, configuration);
            }
            else
            {
                var definition = new ToastNotificationDefinition
                {
                    TextLines = new[] { "Test Toast Notification from Time Track Status UI" }
                };
                var toast = CreateToastFromDefinition(definition);
                await ShowToastNotification(_applicationId, toast);
            }
        }

        private Data GetData()
        {
            var data = new Data();
            var currentTracking = _dataStorage.GetCurrentTrackingActivity();
            var lastTrack = _dataStorage.GetTrackedActivities()
                .OrderByDescending(x => x.Modified)
                .FirstOrDefault();

            data.LastTrackedActivity = lastTrack;
            if (currentTracking != null)
            {
                var trackingSince = currentTracking.GetDuration();
                data.IsTracking = true;
                data.Duration = trackingSince;
                data.CurrentTrackingActivity = currentTracking;
            }
            else
            {
                data.IsTracking = false;
                data.Duration = _timeServiceProvider.Now() - (lastTrack?.Stopped).GetValueOrDefault(DateTime.MinValue);
            }
            return data;
        }

        private ReminderConfiguration GetReminderConfiguration()
        {
            var sectionName = "Reminder";
            var typedConfiguration = new ReminderConfiguration();
            _configuration.Bind(sectionName, typedConfiguration);
            return typedConfiguration;
        }

        private bool IsMatchingTrackingRule(NonIdleNotificationDefinition definition, Data data)
        {
            if (definition.Activities == null)
            {
                return false;
            }

            return definition.Activities.Any(x => string.Equals(x, data.CurrentTrackingActivity.Identifier, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsSilentTime(ReminderConfiguration configuration)
        {
            if (configuration.SilentTimes != null)
            {
                return configuration.SilentTimes.Select(x => new SilentTimeWrapper(x)).Any(x => x.IsMatch(DateTime.Now));
            }

            return false;
        }

        private bool MayRetrigger(string id, TimeSpan retriggerThreshold)
        {
            if (_notificationLogbook.ContainsKey(id))
            {
                var lastTimeSpan = DateTime.Now - _notificationLogbook[id];
                return lastTimeSpan > retriggerThreshold;
            }

            return true;
        }

        private void NotificationTriggered(string key)
        {
            if (!_notificationLogbook.ContainsKey(key))
            {
                _notificationLogbook.Add(key, DateTime.Now);
            }
            _notificationLogbook[key] = DateTime.Now;
        }

        private ToastNotificationDefinition RenderNotificationDefinitionTextLines(ToastNotificationDefinition toastNotificationDefinition)
        {
            var result = (ToastNotificationDefinition)toastNotificationDefinition.Clone();
            var templateContext = new ReminderTextTemplateContext
            {
                Data = GetData(),
                Now = DateTime.Now,
            };
            result.TextLines = toastNotificationDefinition.TextLines.Select(line => _textTemplateRenderer.Render(Environment.ExpandEnvironmentVariables(line), templateContext)).ToArray();
            return result;
        }

        private async Task ShowNotificationByDefinitionName(string name, ReminderConfiguration configuration)
        {
            var definition = configuration.ToastNotificationDefinitions?.SingleOrDefault(x => string.Equals(x.DefinitionName, name, StringComparison.OrdinalIgnoreCase));
            if (definition == null)
            {
                var available = string.Join(", ", configuration.ToastNotificationDefinitions?.Select(x => x.DefinitionName));
                throw new ArgumentException($"The toast notification definition '{name}' is not configured! (Available: {available})");
            }

            var toast = CreateToastFromDefinition(definition);
            await ShowToastNotification(_applicationId, toast);
        }

        private struct Data
        {
            public ITrackingActivity CurrentTrackingActivity { get; set; }
            public TimeSpan Duration { get; set; }
            public bool IsTracking { get; set; }
            public ITrackedActivity LastTrackedActivity { get; set; }
        }

        private struct ReminderTextTemplateContext
        {
            public Data Data { get; set; }

            public DateTime Now { get; set; }
        }

        private class SilentTimeWrapper
        {
            private readonly SilentTimeDefinition _definition;

            public SilentTimeWrapper(SilentTimeDefinition definition)
            {
                _definition = definition;
            }

            public bool IsMatch(DateTime time)
            {
                if (!IsWeekdayMatch(time))
                {
                    return false;
                }

                var currentTime = time.TimeOfDay;
                return currentTime >= _definition.Start && currentTime <= _definition.End;
            }

            public bool IsWeekdayMatch(DateTime time)
            {
                if (_definition.Days != null && _definition.Days.Any())
                {
                    return _definition.Days.Any(x => x == time.DayOfWeek);
                }

                return false;
            }
        }
    }
}