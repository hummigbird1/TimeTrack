using System;
using System.Linq;
using TimeTrack.Application.Common;
using TimeTrack.Console.Interfaces;
using TimeTrack.Console.Options;
using TimeTrack.Interfaces;

namespace TimeTrack.Console.QueryHandling
{
    internal class ParsableNoActiveTrackingSinceQuery : IHandler<QueryOptions, string>
    {
        private static readonly string EmptyDatabaseResult = $"{int.MaxValue}|||";
        private readonly IDataStorage _dataStorage;
        private readonly ITimeServiceProvider _timeServiceProvider;

        public ParsableNoActiveTrackingSinceQuery(IDataStorage dataStorage, ITimeServiceProvider timeServiceProvider)
        {
            _dataStorage = dataStorage;
            _timeServiceProvider = timeServiceProvider;
        }

        public string Handle(QueryOptions argument)
        {
            var currentTracking = _dataStorage.GetCurrentTrackingActivity();
            if (currentTracking != null)
            {
                var trackingSince = currentTracking.GetDuration();
                return $"{0}|{trackingSince.ToOptimizedString(true)}|{currentTracking.Started}|{currentTracking.Identifier}";
            }

            var lastTrack = _dataStorage.GetTrackedActivities()
                .OrderByDescending(x => x.Modified)
                .FirstOrDefault();

            if (lastTrack == null)
            {
                return EmptyDatabaseResult;
            }

            var idlePeriod = _timeServiceProvider.Now() - lastTrack.Stopped;
            var idleSinceRoundedSeconds = (int)Math.Round(idlePeriod.TotalSeconds);
            return $"{idleSinceRoundedSeconds}|{idlePeriod.ToOptimizedString(true)}|{lastTrack.Stopped}|{lastTrack.Identifier}";
        }
    }
}