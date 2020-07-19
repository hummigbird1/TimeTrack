using System;
using TimeTrack.Interfaces;

namespace TimeTrack.Powershell.Cmdlet
{
    public class TrackedActivity : ITrackedActivity
    {
        public DateTime Created { get; internal set; }
        public TimeSpan Duration => Stopped - Started;

        public double DurationSeconds => Duration.TotalSeconds;

        public string Identifier { get; internal set; }

        public DateTime Modified { get; set; }
        public string RecordId { get; internal set; }

        public DateTime Started { get; internal set; }

        public DateTime Stopped { get; internal set; }

        public static TrackedActivity Create(ITrackedActivity trackedActivity)
        {
            return new TrackedActivity
            {
                Identifier = trackedActivity.Identifier,
                RecordId = trackedActivity.RecordId,
                Started = trackedActivity.Started,
                Stopped = trackedActivity.Stopped,
                Created = trackedActivity.Created,
                Modified = trackedActivity.Modified
            };
        }
    }
}