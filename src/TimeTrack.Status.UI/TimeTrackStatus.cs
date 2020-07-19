using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TimeTrack.Interfaces;

namespace TimeTrack.Status.UI
{
    internal class TimeTrackStatus : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private readonly IDataStorage _dataStorage;

        private bool _isTrackingActive;

        private ITrackedActivity _lastTrackedActivity;

        private Dictionary<string, ICollection<string>> _propertyDependencies = new Dictionary<string, ICollection<string>>();

        private Sum _totalThisWeek;
        private Sum _totalThisWeekForLastIdentifier;
        private Sum _totalToday;
        private Sum _totalTodayForLastIdentifier;
        private ITrackingActivity _trackingActivity;

        public TimeTrackStatus(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
            SetupPropertyChangeChain();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        public string CurrentlyTrackingIdentifier
        {
            get
            {
                return TrackingActivity?.Identifier;
            }
        }

        public TimeSpan Duration
        {
            get
            {
                if (TrackingActivity != null)
                {
                    return DateTime.Now - TrackingActivity.Started;
                }
                else if (LastTrackedActivity != null)
                {
                    return DateTime.Now - LastTrackedActivity.Stopped;
                }

                return DateTime.Now - DateTime.MinValue;
            }
        }

        public bool IsTrackingActive { get => _isTrackingActive; set => ChangePropertyValue(ref _isTrackingActive, value); }
        public ITrackedActivity LastTrackedActivity { get => _lastTrackedActivity; set => ChangePropertyValue(ref _lastTrackedActivity, value); }

        public string LastTrackedIdentifier => LastTrackedActivity?.Identifier;
        public Sum TotalThisWeek { get => _totalThisWeek; private set => ChangePropertyValue(ref _totalThisWeek, value); }
        public Sum TotalThisWeekForLastIdentifier { get => _totalThisWeekForLastIdentifier; private set => ChangePropertyValue(ref _totalThisWeekForLastIdentifier, value); }
        public Sum TotalToday { get => _totalToday; private set => ChangePropertyValue(ref _totalToday, value); }
        public Sum TotalTodayForLastIdentifier { get => _totalTodayForLastIdentifier; private set => ChangePropertyValue(ref _totalTodayForLastIdentifier, value); }
        public ITrackingActivity TrackingActivity { get => _trackingActivity; set => ChangePropertyValue(ref _trackingActivity, value); }

        public void ChangePropertyValue<T>(ref T memberField, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(memberField, newValue))
            {
                if (_propertyDependencies.ContainsKey(propertyName))
                {
                    foreach (var e in _propertyDependencies[propertyName])
                    {
                        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(e));
                    }
                }
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));

                memberField = newValue;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                if (_propertyDependencies.ContainsKey(propertyName))
                {
                    foreach (var e in _propertyDependencies[propertyName])
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e));
                    }
                }
            }
        }

        public void Update()
        {
            var last = _dataStorage.GetTrackedActivities()?.OrderByDescending(x => x.Modified).Take(1)?.FirstOrDefault();
            LastTrackedActivity = last;

            var current = _dataStorage.GetCurrentTrackingActivity();
            IsTrackingActive = current != null;
            TrackingActivity = current;

            var monday = FindMonday();
            var identifier = current != null ? current?.Identifier : last?.Identifier;
            TotalTodayForLastIdentifier = new Sum(GetActivities(x => x.Started >= DateTime.Today && x.Stopped < DateTime.Today.AddDays(1) && string.Equals(x.Identifier, identifier)).ToList());

            TotalThisWeekForLastIdentifier = new Sum(GetActivities(x => x.Started >= monday && string.Equals(x.Identifier, identifier)).ToList());

            TotalToday = new Sum(GetActivities(x => x.Started >= DateTime.Today && x.Stopped < DateTime.Today.AddDays(1)).ToList());

            TotalThisWeek = new Sum(GetActivities(x => x.Started >= monday).ToList());
        }

        private DateTime FindMonday()
        {
            var date = DateTime.Today;
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }

            return date;
        }

        private IEnumerable<TrackingActivityContainer> GetActivities(Predicate<ITrackedActivity> filter)
        {
            var trackedActivities = _dataStorage.GetTrackedActivities(filter)
                .Select(x => new TrackingActivityContainer(x));
            var current = _dataStorage.GetCurrentTrackingActivity();
            if (current != null)
            {
                return trackedActivities.Union(new[] { new TrackingActivityContainer(current) });
            }

            return trackedActivities;
        }

        private void SetupPropertyChangeChain()
        {
            _propertyDependencies.Add(nameof(TrackingActivity), new[] { nameof(CurrentlyTrackingIdentifier), nameof(Duration) });
            _propertyDependencies.Add(nameof(LastTrackedActivity), new[] { nameof(Duration), nameof(LastTrackedIdentifier) });
        }

        public struct Sum
        {
            public Sum(ICollection<TrackingActivityContainer> trackingActivities)
            {
                TotalDuration = TimeSpan.FromTicks(trackingActivities.Sum(x => x.Duration.Ticks));
                EventCount = trackingActivities.Count;
                DistinctEventCount = trackingActivities.GroupBy(x => x.Identifier).Count();
            }

            public int DistinctEventCount { get; }
            public int EventCount { get; }
            public TimeSpan TotalDuration { get; }
        }
    }
}