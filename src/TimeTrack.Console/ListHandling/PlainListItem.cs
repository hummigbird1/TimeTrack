using System;
using TimeTrack.Console.Output.Attributes;

namespace TimeTrack.Console.ListHandling
{
    public struct PlainListItem
    {
        public PlainListItem(TrackingActivityContainer item)
        {
            RecordId = item.RecordId;
            Identifier = item.Identifier;
            Started = item.Started;
            Stopped = item.Stopped;
            Created = item.Created;
            Modified = item.Modified;
            IsCurrentlyTracking = item.IsUnfinished;
        }

        [ListHeaderText("Created")]
        public DateTime Created { get; }

        [ListHeaderText("Duration")]
        public TimeSpan Duration => Stopped - Started;

        [ListHeaderText("Identifier")]
        public string Identifier { get; }

        public bool IsCurrentlyTracking { get; }

        [ListHeaderText("Modified")]
        public DateTime Modified { get; }

        [ListHeaderText("RecordId")]
        public string RecordId { get; }

        [ListHeaderText("Started")]
        public DateTime Started { get; }

        [ListHeaderText("Stopped")]
        public DateTime Stopped { get; }
    }
}