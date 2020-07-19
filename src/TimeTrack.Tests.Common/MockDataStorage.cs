using System;
using System.Collections.Generic;
using System.Linq;
using TimeTrack.Interfaces;

namespace TimeTrack.Tests.Common
{
    public class MockDataStorage : IDataStorage
    {
        private List<ITrackedActivity> _trackedActivities = new List<ITrackedActivity>();

        private ITrackingActivity _trackingActivity;

        public MockDataStorage()
        {
        }

        public MockDataStorage(IEnumerable<ITrackedActivity> trackedActivities)
        {
            _trackedActivities.AddRange(trackedActivities);
        }

        public ITrackingActivity TrackingActivity
        {
            set
            {
                _trackingActivity = value;
            }
        }

        public ITrackingActivity GetCurrentTrackingActivity()
        {
            return _trackingActivity;
        }

        public IEnumerable<ITrackedActivity> GetTrackedActivities(Predicate<ITrackedActivity> filterPredicate = null)
        {
            if (filterPredicate != null)
            {
                return _trackedActivities.Where(x => filterPredicate(x));
            }

            return _trackedActivities;
        }

        public ITrackedActivity SaveTrackedActivity(ITrackedActivity trackedActivity)
        {
            _trackedActivities.Add(trackedActivity);
            return trackedActivity;
        }

        public void UpdateCurrentTrackingActivity(ITrackingActivity activity)
        {
            _trackingActivity = activity;
        }
    }
}