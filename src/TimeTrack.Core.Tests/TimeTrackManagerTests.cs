using System;
using TimeTrack.Tests.Common;
using Xunit;

namespace TimeTrack.Core.Tests
{
    public class TimeTrackManagerTests
    {
        [Fact]
        public void Discard_WitActive_NoNewActivity()
        {
            var timeService = new MockTimeServiceProvider();
            var config = new TimeTrackManagerConfiguration();
            var storage = new MockDataStorage();
            var manager = new TimeTrackManager(storage, timeService, config);

            manager.StartTrackingNow("Test");
            Assert.NotNull(manager.CurrentTrackingActivity);
            Assert.Empty(storage.GetTrackedActivities());

            manager.Discard();
            Assert.Null(manager.CurrentTrackingActivity);
            Assert.Empty(storage.GetTrackedActivities());
        }

        [Fact]
        public void Discard_WithoutActive_NoError()
        {
            var timeService = new MockTimeServiceProvider();
            var config = new TimeTrackManagerConfiguration();
            var storage = new MockDataStorage();
            var manager = new TimeTrackManager(storage, timeService, config);

            Assert.Null(manager.CurrentTrackingActivity);
            Assert.Empty(storage.GetTrackedActivities());

            manager.Discard();
            Assert.Null(manager.CurrentTrackingActivity);
        }

        [Fact]
        public void StartTrackingNow_StartsTracking()
        {
            var timeService = new MockTimeServiceProvider();
            var config = new TimeTrackManagerConfiguration();
            var storage = new MockDataStorage();
            var manager = new TimeTrackManager(storage, timeService, config);

            manager.StartTrackingNow("Test");

            Assert.NotNull(manager.CurrentTrackingActivity);
            Assert.Empty(storage.GetTrackedActivities());
        }

        [Fact]
        public void StartTrackingNow_WithActive_StartsTrackingAndWriteNewActivity()
        {
            var timeService = new MockTimeServiceProvider();
            var config = new TimeTrackManagerConfiguration();
            var storage = new MockDataStorage();
            var manager = new TimeTrackManager(storage, timeService, config);

            manager.StartTrackingNow("Test");

            Assert.NotNull(manager.CurrentTrackingActivity);
            Assert.Empty(storage.GetTrackedActivities());

            manager.StartTrackingNow("Test 2");
            Assert.NotNull(manager.CurrentTrackingActivity);
            Assert.Single(storage.GetTrackedActivities());
        }

        [Fact]
        public void StopTracking_WithAutoDiscard_Discards()
        {
            var timeService = new MockTimeServiceProvider(DateTime.Now);
            var config = new TimeTrackManagerConfiguration
            {
                AutoDiscardThreshold = TimeSpan.FromSeconds(10)
            };
            var storage = new MockDataStorage
            {
            };
            storage.UpdateCurrentTrackingActivity(new MockTrackingActivity { Started = timeService.Now().AddSeconds(-10) });

            var manager = new TimeTrackManager(storage, timeService, config);
            Assert.NotNull(manager.CurrentTrackingActivity);
            Assert.Empty(storage.GetTrackedActivities());

            manager.StopTrackingNow();
            Assert.Null(manager.CurrentTrackingActivity);
            Assert.Empty(storage.GetTrackedActivities());
        }

        [Fact]
        public void StopTracking_WithAutoDiscard_DoesNotDiscard()
        {
            var timeService = new MockTimeServiceProvider(DateTime.Now);
            var config = new TimeTrackManagerConfiguration
            {
                AutoDiscardThreshold = TimeSpan.FromSeconds(10)
            };
            var storage = new MockDataStorage
            {
            };
            storage.UpdateCurrentTrackingActivity(new MockTrackingActivity { Started = timeService.Now().AddSeconds(-11) });

            var manager = new TimeTrackManager(storage, timeService, config);
            Assert.NotNull(manager.CurrentTrackingActivity);
            Assert.Empty(storage.GetTrackedActivities());

            manager.StopTrackingNow();
            Assert.Null(manager.CurrentTrackingActivity);
            Assert.Single(storage.GetTrackedActivities());
        }

        [Fact]
        public void StopTrackingNow_WithActive_WritesNewActivity()
        {
            var timeService = new MockTimeServiceProvider();
            var config = new TimeTrackManagerConfiguration();
            var storage = new MockDataStorage();
            var manager = new TimeTrackManager(storage, timeService, config);

            manager.StartTrackingNow("Test");

            Assert.NotNull(manager.CurrentTrackingActivity);
            manager.StopTrackingNow();
            Assert.Single(storage.GetTrackedActivities());
        }

        [Fact]
        public void StopTrackingNow_WithoutActive_NoError()
        {
            var timeService = new MockTimeServiceProvider();
            var config = new TimeTrackManagerConfiguration();
            var storage = new MockDataStorage();
            var manager = new TimeTrackManager(storage, timeService, config);

            Assert.Null(manager.CurrentTrackingActivity);
            Assert.Empty(storage.GetTrackedActivities());

            manager.StopTrackingNow();
            Assert.Null(manager.CurrentTrackingActivity);
            Assert.Empty(storage.GetTrackedActivities());
        }
    }
}