using System;
using TimeTrack.Console.Output.Attributes;
using TimeTrack.Application.Common;

namespace TimeTrack.Console.ListHandling
{
    public struct SummaryListItem
    {
        [ListHeaderText("Average Duration")]
        public TimeSpan AverageDuration { get; set; }

        public bool ContainsCurrentlyTracking { get; set; }

        [ListHeaderText("From")]
        public DateTime From { get; set; }

        [ListHeaderText("Identifier")]
        public string Identifier { get; set; }

        [ColumnAlignment(true)]
        [ListHeaderText("Records")]
        public int RecordCount { get; set; }

        [ListHeaderText("To")]
        public DateTime To { get; set; }

        [ListHeaderText("TotalDuration")]
        public TimeSpan TotalDuration { get; set; }

        [ColumnAlignment(true)]
        [ListHeaderText("TotalDuration (Jira)")]
        public string TotalDurationJira => TotalDuration.GetDurationAsJiraLoggableTime();
    }
}