using System;
using System.IO;
using System.Linq;
using TimeTrack.Application.Common;
using TimeTrack.Interfaces;

namespace TimeTrack.Console.Export
{
    internal class SummarizedListExporter : ExporterBase, IExporter
    {
        public SummarizedListExporter(IDataStorageQueryProvider dataStorageQueryProvider) : base(dataStorageQueryProvider)
        {
        }

        public void Export(bool header, string separator, DateTime from, DateTime to, Stream stream)
        {
            var entries = GetTrackedActivitiesInTimeRange(from, to);
            var summarizedEntries = entries.GroupBy(x => x.Identifier)
                .Select(x => (Identifier: x.Key, Total: TimeSpan.FromTicks(x.Sum(e => e.GetDuration().Ticks))))
                .OrderByDescending(x => x.Total);

            if (header)
            {
                WriteAsLineToStream($"Identifier{separator}TotalDuration{separator}TotalDurationAsJiraLoggableTime", stream);
            }

            foreach (var e in summarizedEntries.OrderByDescending(x => x.Total))
            {
                var line = $"{e.Identifier}{separator}{e.Total}{separator}{e.Total.GetDurationAsJiraLoggableTime()}";
                WriteAsLineToStream(line, stream);
            }
        }
    }
}