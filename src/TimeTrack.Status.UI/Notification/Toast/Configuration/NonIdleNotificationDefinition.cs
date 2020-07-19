using System;

namespace TimeTrack.Status.UI.Notification.Toast.Configuration
{
    public class NonIdleNotificationDefinition
    {
        public string[] Activities { get; set; }

        public string DefinitionName { get; set; }
        public TimeSpan NotificationThreshhold { get; set; } = TimeSpan.FromMinutes(5);

        public TimeSpan RetriggerThreshhold { get; set; } = TimeSpan.FromMinutes(5);
    }
}