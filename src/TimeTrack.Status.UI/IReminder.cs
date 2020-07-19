using System.Threading.Tasks;

namespace TimeTrack.Status.UI
{
    internal interface IReminder
    {
        bool DisableIdleReminder { get; set; }

        bool DisableTrackingReminder { get; set; }

        void OnTrackingStatusChanged();

        Task TestNotificationAsync();

        Task UpdateAsync();

        bool NotificationsActive { get; }
    }
}