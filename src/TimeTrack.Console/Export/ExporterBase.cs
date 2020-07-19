using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TimeTrack.Interfaces;

namespace TimeTrack.Console.Export
{
    internal abstract class ExporterBase
    {
        private readonly IDataStorageQueryProvider _dataStorageQueryProvider;

        protected ExporterBase(IDataStorageQueryProvider dataStorageQueryProvider)
        {
            _dataStorageQueryProvider = dataStorageQueryProvider;
        }

        protected IEnumerable<ITrackedActivity> GetTrackedActivitiesInTimeRange(DateTime from, DateTime to)
        {
            return _dataStorageQueryProvider.GetTrackedActivitiesInTimeRange(from, to).OrderBy(x => x.Stopped).ToList();
        }

        protected void WriteAsLineToStream(string line, Stream stream)
        {
            var data = line + Environment.NewLine;
            var bytes = Encoding.UTF8.GetBytes(data);
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}