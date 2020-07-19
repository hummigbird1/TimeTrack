using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text;
using TimeTrack.Interfaces;

namespace TimeTrack.Application.Common
{
    public static class ExtensionMethods
    {
        public static T BindConfigurationSection<T>(this IConfiguration configuration, string sectionName) where T : class, new()
        {
            var section = configuration.GetSection(sectionName);
            if (!(section?.GetChildren().Any()).GetValueOrDefault(false))
            {
                throw new ArgumentOutOfRangeException(nameof(configuration), $"Configuration does not contain required section '{sectionName}'!");
            }

            var typedConfiguration = new T();
            configuration.Bind(sectionName, typedConfiguration);
            return typedConfiguration;
        }

        public static TimeSpan GetDuration(this ITrackedActivity trackedActivity)
        {
            return trackedActivity.Stopped - trackedActivity.Started;
        }

        public static TimeSpan GetDuration(this ITrackingActivity trackingActivity)
        {
            return DateTime.Now - trackingActivity.Started;
        }

        public static string GetDurationAsJiraLoggableTime(this TimeSpan timeSpan)
        {
            var stringBuilder = new StringBuilder();
            if (timeSpan.Days > 0)
            {
                stringBuilder.AppendFormat("{0}d ", timeSpan.Days);
            }
            if (timeSpan.Hours > 0)
            {
                stringBuilder.AppendFormat("{0}h ", timeSpan.Hours);
            }
            if (timeSpan.Minutes > 0)
            {
                var minutes = timeSpan.Minutes;
                if (timeSpan.Seconds >= 30)
                {
                    minutes++;
                }
                stringBuilder.AppendFormat("{0}m ", minutes);
            }
            else if (timeSpan.Seconds > 30)
            {
                stringBuilder.AppendFormat("1m ");
            }

            return stringBuilder.ToString();
        }

        public static string ToOptimizedString(this TimeSpan timeSpan, bool noDays)
        {
            if (noDays)
            {
                return string.Format(new OptimizedTimeSpanFormatProvider(), "{0:NoDays}", timeSpan);
            }

            return string.Format(new OptimizedTimeSpanFormatProvider(), "{0}", timeSpan);
        }
    }
}