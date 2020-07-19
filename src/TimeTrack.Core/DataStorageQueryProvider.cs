using System;
using System.Collections.Generic;
using TimeTrack.Interfaces;

namespace TimeTrack.Core
{
    public class DataStorageQueryProvider : IDataStorageQueryProvider
    {
        public DataStorageQueryProvider(IDataStorage dataStorage)
        {
            DataStorage = dataStorage ?? throw new ArgumentNullException(nameof(dataStorage));
        }

        public IDataStorage DataStorage { get; }

        public IEnumerable<ITrackedActivity> GetTrackedActivitiesInTimeRange(DateTime start, DateTime stop)
        {
            return DataStorage.GetTrackedActivities(TimeRangePredicate(start, stop));
        }

        private Predicate<ITrackedActivity> TimeRangePredicate(DateTime start, DateTime stop)
        {
            return new Predicate<ITrackedActivity>(x => x.Started >= start && x.Stopped <= stop);
        }
    }
}