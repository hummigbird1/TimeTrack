using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeTrack.Interfaces;

namespace TimeTrack.LiteDb
{
    public sealed class LiteDbDataStorage : IDataStorage, IDisposable
    {
        private readonly LiteDatabase _database;
        private readonly LiteDbStorageTypeMapper _mapper;

        public LiteDbDataStorage(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "Empty connection string was specified!");
            }

            _database = new LiteDatabase(connectionString);
            _mapper = new LiteDbStorageTypeMapper();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ITrackingActivity GetCurrentTrackingActivity()
        {
            var c = _database.GetCollection<TrackingActivity>();
            return c.FindOne(x => true);
        }

        public IEnumerable<ITrackedActivity> GetTrackedActivities(Predicate<ITrackedActivity> filterPredicate = null)
        {
            var c = _database.GetCollection<TrackedActivity>();
            if (filterPredicate != null)
            {
                return c.FindAll().Where(x => filterPredicate(x));
            }
            return c.FindAll();
        }

        public ITrackedActivity SaveTrackedActivity(ITrackedActivity trackedActivity)
        {
            if (trackedActivity == null)
            {
                throw new ArgumentNullException(nameof(trackedActivity));
            }

            var storageTrackedActivity = _mapper.Map<ITrackedActivity, TrackedActivity>(trackedActivity);
            var c = _database.GetCollection<TrackedActivity>();
            c.Insert(storageTrackedActivity);
            return storageTrackedActivity;
        }

        public void UpdateCurrentTrackingActivity(ITrackingActivity activity)
        {
            var c = _database.GetCollection<TrackingActivity>();
            if (activity == null)
            {
                c.DeleteAll();
            }
            else
            {
                var storageActivity = _mapper.Map<ITrackingActivity, TrackingActivity>(activity);
                c.Upsert(storageActivity);
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _database.Dispose();
            }
        }
    }
}