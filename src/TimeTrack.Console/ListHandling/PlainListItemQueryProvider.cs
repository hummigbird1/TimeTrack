using System;
using System.Collections.Generic;
using System.Linq;
using TimeTrack.Console.Interfaces;

namespace TimeTrack.Console.ListHandling
{
    public class PlainListItemQueryProvider : IActivityQueryProvider<PlainListItem>
    {
        private readonly IActivityQueryProvider<TrackingActivityContainer> _activityQueryProvider;

        public PlainListItemQueryProvider(IActivityQueryProvider<TrackingActivityContainer> activityQueryProvider)
        {
            _activityQueryProvider = activityQueryProvider;
        }

        public IEnumerable<PlainListItem> GetAllActivitiesInTimerange(DateTime from, DateTime to)
        {
            return _activityQueryProvider.GetAllActivitiesInTimerange(from, to)
                .Select(x => new PlainListItem(x));
        }
    }
}