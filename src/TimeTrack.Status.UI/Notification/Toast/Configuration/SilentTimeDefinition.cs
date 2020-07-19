using System;

namespace TimeTrack.Status.UI.Notification.Toast.Configuration
{
    public class SilentTimeDefinition
    {
        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }

        public DayOfWeek[] Days { get; set; }
    }
}