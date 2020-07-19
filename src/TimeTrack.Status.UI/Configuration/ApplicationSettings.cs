using System;

namespace TimeTrack.Status.UI.Configuration
{
    public class ApplicationSettings
    {
        public bool DisableNotifications { get; set; } = false;

        public TimeSpan UpdateInterval { get; set; } = TimeSpan.FromSeconds(1);
    }
}