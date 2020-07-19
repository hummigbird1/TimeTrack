using System;
using System.Collections.Generic;

namespace TimeTrack.Interfaces
{
    public interface IDataStorageQueryProvider
    {
        IDataStorage DataStorage { get; }

        IEnumerable<ITrackedActivity> GetTrackedActivitiesInTimeRange(DateTime start, DateTime stop);
    }
}