using System;

namespace TimeTrack.Interfaces
{
    public interface ITrackingActivity : IActivity, IStorageRecord
    {
        DateTime Started { get; set; }
    }
}