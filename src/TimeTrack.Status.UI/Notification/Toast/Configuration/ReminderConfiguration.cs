namespace TimeTrack.Status.UI.Notification.Toast.Configuration
{
    public class ReminderConfiguration
    {
        public SilentTimeDefinition[] SilentTimes { get; set; }

        public IdleNotificationDefinition OnIdle { get; set; }

        public string OnTest { get; set; }

        public NonIdleNotificationDefinition[] OnTracking { get; set; }

        public ToastNotificationDefinition[] ToastNotificationDefinitions { get; set; }
    }
}