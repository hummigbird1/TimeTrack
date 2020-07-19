using System;
using System.IO;
using TimeTrack.Tests.Common;
using Xunit;

namespace TimeTrack.LiteDb.Tests
{
    public class LiteDbDataStorageTests
    {
        [Fact]
        public void GetCurrentTrackingActivity()
        {
            using (var liteDbDataStorage = CreateForTest(nameof(GetCurrentTrackingActivity)))
            {
                Assert.Null(liteDbDataStorage.GetCurrentTrackingActivity());
            }
        }

        [Fact]
        public void SaveTrackedActivity()
        {
            using (var liteDbDataStorage = CreateForTest(nameof(SaveTrackedActivity)))
            {
                Assert.Empty(liteDbDataStorage.GetTrackedActivities());

                var mockActivity = new MockTrackedActivity() { Identifier = "Test", Started = DateTime.MinValue, Stopped = DateTime.MaxValue };
                var savedActivity = liteDbDataStorage.SaveTrackedActivity(mockActivity);
                Assert.NotNull(savedActivity);
                Assert.NotSame(mockActivity, savedActivity);
                Assert.Equal(mockActivity.Identifier, savedActivity.Identifier);
                Assert.NotEqual(mockActivity.RecordId, savedActivity.RecordId);
                Assert.Equal(mockActivity.Started, savedActivity.Started);
                Assert.Equal(mockActivity.Stopped, savedActivity.Stopped);

                Assert.Single(liteDbDataStorage.GetTrackedActivities());
            }
        }

        [Fact]
        public void SaveTrackedActivity_NotNull()
        {
            using (var liteDbDataStorage = CreateForTest(nameof(SaveTrackedActivity_NotNull)))
            {
                Assert.Throws<ArgumentNullException>(() => liteDbDataStorage.SaveTrackedActivity(null));
            }
        }

        [Fact]
        public void UpdateCurrentTrackingActivity()
        {
            using (var liteDbDataStorage = CreateForTest(nameof(UpdateCurrentTrackingActivity)))
            {
                Assert.Null(liteDbDataStorage.GetCurrentTrackingActivity());

                var activity = new MockTrackingActivity { Identifier = "Test", Started = DateTime.Now };
                liteDbDataStorage.UpdateCurrentTrackingActivity(activity);

                var saved = liteDbDataStorage.GetCurrentTrackingActivity();
                Assert.NotNull(saved);
                Assert.NotSame(activity, saved);
                Assert.Equal(activity.Identifier, saved.Identifier);

                // Lite DB does not store the full precision of the DateTime Milliseconds
                var min = activity.Started.AddMilliseconds(activity.Started.Millisecond * -1);
                Assert.InRange(saved.Started, min, activity.Started);
            }
        }

        [Fact]
        public void UpdateCurrentTrackingActivity_Reset()
        {
            using (var liteDbDataStorage = CreateForTest(nameof(UpdateCurrentTrackingActivity_Reset)))
            {
                Assert.Null(liteDbDataStorage.GetCurrentTrackingActivity());

                var activity = new MockTrackingActivity();
                liteDbDataStorage.UpdateCurrentTrackingActivity(activity);

                Assert.NotNull(liteDbDataStorage.GetCurrentTrackingActivity());

                liteDbDataStorage.UpdateCurrentTrackingActivity(null);
                Assert.Null(liteDbDataStorage.GetCurrentTrackingActivity());
            }
        }

        private LiteDbDataStorage CreateForTest(string testName)
        {
            string fileName = $"TimeTrack_{testName}.db";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            return new LiteDbDataStorage(fileName);
        }
    }
}