using System;
using System.Linq;
using TimeTrack.Application.Common.Interfaces;
using TimeTrack.Console.Interfaces;
using TimeTrack.Console.Options;
using TimeTrack.Interfaces;

namespace TimeTrack.Console.QueryHandling
{
    internal class QueryIdentifierList : IHandler<QueryOptions, string>
    {
        private readonly IActivityQueryProvider<TrackingActivityContainer> _activityQueryProvider;
        private readonly IDateTimeStringParser _dateTimeStringParser;
        private readonly ITimeServiceProvider _timeServiceProvider;

        public QueryIdentifierList(ITimeServiceProvider timeServiceProvider, IServiceTypeSelector<IDateTimeStringParser> dateTimeStringParser, IActivityQueryProvider<TrackingActivityContainer> activityQueryProvider)
        {
            _timeServiceProvider = timeServiceProvider;
            _dateTimeStringParser = dateTimeStringParser.GetRequired();
            _activityQueryProvider = activityQueryProvider;
        }

        public string Handle(QueryOptions argument)
        {
            return CreateOutput(argument);
        }

        private string CreateOutput(QueryOptions options)
        {
            var timeRange = TimeRange.Create(_dateTimeStringParser);
            var to = _timeServiceProvider.Now();
            var from = to.AddDays(-3).Date;
            timeRange.ParseFrom(options.From, from);
            timeRange.ParseTo(options.To, to);
            timeRange.ThrowExceptionIfInvalid();

            var identifiers = _activityQueryProvider.GetAllActivitiesInTimerange(timeRange.From, timeRange.To)
                .Where(x => !string.IsNullOrWhiteSpace(x.Identifier))
                .OrderByDescending(x => x.Modified)
                .Select(x => x.Identifier)
                .Distinct(StringComparer.Ordinal);

            return string.Join(Environment.NewLine, identifiers);
        }
    }
}