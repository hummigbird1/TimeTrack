using System;
using System.Collections.Generic;

namespace TimeTrack.Interfaces
{
    public interface IDataStorage
    {
        ITrackingActivity GetCurrentTrackingActivity();

        IEnumerable<ITrackedActivity> GetTrackedActivities(Predicate<ITrackedActivity> filterPredicate = null);

        ITrackedActivity SaveTrackedActivity(ITrackedActivity trackedActivity);

        void UpdateCurrentTrackingActivity(ITrackingActivity activity);
    }
}