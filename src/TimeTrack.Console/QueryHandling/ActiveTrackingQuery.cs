using TimeTrack.Console.Interfaces;
using TimeTrack.Console.Options;
using TimeTrack.Core;
using TimeTrack.Interfaces;

namespace TimeTrack.Console.QueryHandling
{
    internal abstract class ActiveTrackingQuery : IHandler<QueryOptions, string>
    {
        private readonly TimeTrackManager _timeTrackManager;

        public ActiveTrackingQuery(TimeTrackManager timeTrackManager)
        {
            _timeTrackManager = timeTrackManager;
        }

        public string Handle(QueryOptions argument)
        {
            var e = _timeTrackManager.CurrentTrackingActivity;
            return CreateOutput(e);
        }

        protected abstract string CreateOutput(ITrackingActivity trackingActivity);
    }
}