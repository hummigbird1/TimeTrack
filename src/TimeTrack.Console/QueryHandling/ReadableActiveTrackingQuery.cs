using TimeTrack.Application.Common;
using TimeTrack.Core;
using TimeTrack.Interfaces;

namespace TimeTrack.Console.QueryHandling
{
    internal class ReadableActiveTrackingQuery : ActiveTrackingQuery
    {
        public ReadableActiveTrackingQuery(TimeTrackManager timeTrackManager) : base(timeTrackManager)
        {
        }

        protected override string CreateOutput(ITrackingActivity trackingActivity)
        {
            if (trackingActivity != null)
            {
                return $"Tracking: '{trackingActivity.Identifier}' {trackingActivity.Started} => {trackingActivity.GetDuration().ToOptimizedString(true)} {trackingActivity.GetDuration().GetDurationAsJiraLoggableTime()}";
            }
            else
            {
                return "Tracking: <NOTHING>";
            }
        }
    }
}