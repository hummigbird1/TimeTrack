using System;
using System.Collections.Generic;
using System.Linq;
using TimeTrack.Console.Interfaces;
using TimeTrack.Interfaces;

namespace TimeTrack.Console
{
    public class CompositeActivityQueryProvider : IActivityQueryProvider<TrackingActivityContainer>
    {
        private readonly IDataStorageQueryProvider _dataStorageQueryProvider;
        private readonly ITimeServiceProvider _timeServiceProvider;

        public CompositeActivityQueryProvider(IDataStorageQueryProvider dataStorageQueryProvider, ITimeServiceProvider timeServiceProvider)
        {
            _dataStorageQueryProvider = dataStorageQueryProvider;
            _timeServiceProvider = timeServiceProvider;
        }

        public IEnumerable<TrackingActivityContainer> GetAllActivitiesInTimerange(DateTime from, DateTime to)
        {
            var trackedActivities = _dataStorageQueryProvider.GetTrackedActivitiesInTimeRange(from, to)
                .Select(x => new TrackingActivityContainer(x));
            var current = _dataStorageQueryProvider.DataStorage.GetCurrentTrackingActivity();
            if (current != null && IsWithinTimerange(current, from, to))
            {
                return trackedActivities.Union(new[] { new TrackingActivityContainer(current, _timeServiceProvider) });
            }

            return trackedActivities;
        }

        private bool IsWithinTimerange(ITrackingActivity trackingActivity, DateTime from, DateTime to)
        {
            return trackingActivity.Started >= from && _timeServiceProvider.Now() <= to;
        }
    }
}