using System;

namespace TimeTrack.Interfaces
{
    public interface ITrackedActivity : IActivity, IStorageRecord
    {
        DateTime Started { get; }
        DateTime Stopped { get; }
    }
}