using System;
using System.IO;
using System.Linq;
using TimeTrack.Interfaces;
using TimeTrack.Application.Common;

namespace TimeTrack.Console.Export
{
    internal class PlainListExporter : ExporterBase, IExporter
    {
        public PlainListExporter(IDataStorageQueryProvider dataStorageQueryProvider) : base(dataStorageQueryProvider)
        {
        }

        public void Export(bool header, string separator, DateTime from, DateTime to, Stream stream)
        {
            var entries = GetTrackedActivitiesInTimeRange(from, to).OrderBy(x => x.Modified);

            if (header)
            {
                WriteAsLineToStream($"RecordId{separator}Identifier{separator}Started{separator}Stopped{separator}Duration{separator}DurationAsJiraLoggableTime", stream);
            }
            foreach (var e in entries)
            {
                var line = $"{e.RecordId}{separator}{e.Identifier}{separator}{e.Started}{separator}{e.Stopped}{separator}{e.GetDuration()}{separator}{e.GetDuration().GetDurationAsJiraLoggableTime()}";
                WriteAsLineToStream(line, stream);
            }
        }
    }
}