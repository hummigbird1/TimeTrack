using System;
using TimeTrack.Interfaces;

namespace TimeTrack.Console
{
    public struct TrackingActivityContainer : ITrackedActivity
    {
        public TrackingActivityContainer(ITrackedActivity trackedActivity)
        {
            IsUnfinished = false;
            Started = trackedActivity.Started;
            Stopped = trackedActivity.Stopped;
            Identifier = trackedActivity.Identifier;
            RecordId = trackedActivity.RecordId;
            Modified = trackedActivity.Modified;
            Created = trackedActivity.Created;
        }

        public TrackingActivityContainer(ITrackingActivity trackedActivity, ITimeServiceProvider timeServiceProvider)
        {
            IsUnfinished = true;
            Started = trackedActivity.Started;
            Stopped = timeServiceProvider.Now();
            Identifier = trackedActivity.Identifier;
            RecordId = trackedActivity.RecordId;
            Modified = trackedActivity.Modified;
            Created = trackedActivity.Created;
        }

        public DateTime Created { get; }

        public TimeSpan Duration => Stopped - Started;

        public string Identifier { get; }
        public bool IsUnfinished { get; }
        public DateTime Modified { get; set; }
        public string RecordId { get; }
        public DateTime Started { get; }

        public DateTime Stopped { get; }
    }
}