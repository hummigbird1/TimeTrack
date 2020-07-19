using System;
using System.Collections.Generic;
using System.Linq;
using TimeTrack.Console.Interfaces;

namespace TimeTrack.Console.ListHandling
{
    public class SummaryListItemQueryProvider : IActivityQueryProvider<SummaryListItem>
    {
        private readonly IActivityQueryProvider<TrackingActivityContainer> _activityQueryProvider;

        public SummaryListItemQueryProvider(IActivityQueryProvider<TrackingActivityContainer> activityQueryProvider)
        {
            _activityQueryProvider = activityQueryProvider;
        }

        public IEnumerable<SummaryListItem> GetAllActivitiesInTimerange(DateTime from, DateTime to)
        {
            return _activityQueryProvider.GetAllActivitiesInTimerange(from, to)
                .GroupBy(x => x.Identifier)
                .Select(Convert);
        }

        private SummaryListItem Convert(IGrouping<string, TrackingActivityContainer> group)
        {
            return new SummaryListItem
            {
                Identifier = group.Key,
                TotalDuration = TimeSpan.FromTicks(group.Sum(x => x.Duration.Ticks)),
                AverageDuration = TimeSpan.FromTicks(group.Sum(x => x.Duration.Ticks) / group.Count()),
                ContainsCurrentlyTracking = group.Any(x => x.IsUnfinished),
                RecordCount = group.Count(),
                From = group.Min(x => x.Started),
                To = group.Max(x => x.Stopped)
            };
        }
    }
}