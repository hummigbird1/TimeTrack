using System;
using System.Linq;
using TimeTrack.Core;
using TimeTrack.Tests.Common;
using Xunit;

namespace TimeTrack.Console.Plugin.ResumeCommand.Tests
{
    public class ResumeCommandTests
    {
        [Theory]
        [InlineData("-1")]
        [InlineData("A")]
        public void ResumeActivity_Command_ArgumentExceptions(string commandText)
        {
            var storage = new MockDataStorage();

            var timeTrackManager = new TimeTrackManager(storage, new MockTimeServiceProvider(), new TimeTrackManagerConfiguration { });

            var command = new ResumeCommand(storage, timeTrackManager);
            Assert.Throws<ArgumentException>(() => command.Execute(commandText));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ResumeActivity_Command_ArgumentNullExceptions(string commandText)
        {
            var storage = new MockDataStorage();

            var timeTrackManager = new TimeTrackManager(storage, new MockTimeServiceProvider(), new TimeTrackManagerConfiguration { });

            var command = new ResumeCommand(storage, timeTrackManager);
            Assert.Throws<ArgumentNullException>(() => command.Execute(commandText));
        }

        [Fact]
        public void ResumeActivity_Command0_ReturnsLastTrackedActivity()
        {
            var storage = new MockDataStorage(new[] {
                new MockTrackedActivity{ Identifier = "To be skipped" , Modified = DateTime.Now},
                new MockTrackedActivity{ Identifier = "1 Activity before" , Modified = DateTime.Now.AddMinutes(-1) }

            });
            var timeTrackManager = new TimeTrackManager(storage, new MockTimeServiceProvider(), new TimeTrackManagerConfiguration { });

            Assert.Null(timeTrackManager.CurrentTrackingActivity);

            var command = new ResumeCommand(storage, timeTrackManager);
            command.Execute("0");

            var current = timeTrackManager.CurrentTrackingActivity;
            Assert.NotNull(current);
            Assert.Equal("To be skipped", current.Identifier);

        }

        [Fact]
        public void ResumeActivity_Command0_WithEmptyStorage_DoesNotStart()
        {
            var storage = new MockDataStorage();

            var timeTrackManager = new TimeTrackManager(storage, new MockTimeServiceProvider(), new TimeTrackManagerConfiguration { });

            var command = new ResumeCommand(storage, timeTrackManager);
            command.Execute("0");

            var current = timeTrackManager.CurrentTrackingActivity;
            Assert.Null(current);
            Assert.Empty(storage.GetTrackedActivities());
        }

        [Fact]
        public void ResumeActivity_Command0WithCurrentTracking_ReturnsLastTrackedActivity()
        {
            var storage = new MockDataStorage(new[] {
                new MockTrackedActivity{ Identifier = "1 Activity before" , Modified = DateTime.Now},
                new MockTrackedActivity{ Identifier = "2 Activity before" , Modified = DateTime.Now.AddMinutes(-1) }

            });
            storage.TrackingActivity = new MockTrackingActivity { Identifier = "To be skipped" };

            var timeTrackManager = new TimeTrackManager(storage, new MockTimeServiceProvider(), new TimeTrackManagerConfiguration { });

            Assert.Equal(2, storage.GetTrackedActivities().Count());
            Assert.NotNull(timeTrackManager.CurrentTrackingActivity);
            Assert.Equal("To be skipped", timeTrackManager.CurrentTrackingActivity.Identifier);

            var command = new ResumeCommand(storage, timeTrackManager);
            command.Execute("0");

            var current = timeTrackManager.CurrentTrackingActivity;
            Assert.NotNull(current);
            Assert.Equal("1 Activity before", current.Identifier);
            Assert.Equal(3, storage.GetTrackedActivities().Count());
        }

        [Fact]
        public void ResumeActivity_CommandGreaterRecordCounter_DoesNotStart()
        {
            var storage = new MockDataStorage(new[] {
                new MockTrackedActivity{ Identifier = "1 Activity before" , Modified = DateTime.Now},
                new MockTrackedActivity{ Identifier = "2 Activity before" , Modified = DateTime.Now.AddMinutes(-1) }

            });

            var timeTrackManager = new TimeTrackManager(storage, new MockTimeServiceProvider(), new TimeTrackManagerConfiguration { });

            var command = new ResumeCommand(storage, timeTrackManager);
            command.Execute("3");

            var current = timeTrackManager.CurrentTrackingActivity;
            Assert.Null(current);
            Assert.Equal(2, storage.GetTrackedActivities().Count());

        }

        [Fact]
        public void ResumeActivity_CommandLast_ReturnsLastTrackedActivity()
        {
            var storage = new MockDataStorage(new[] {
                new MockTrackedActivity{ Identifier = "To be skipped" , Modified = DateTime.Now},
                new MockTrackedActivity{ Identifier = "1 Activity before" , Modified = DateTime.Now.AddMinutes(-1) }

            });
            var timeTrackManager = new TimeTrackManager(storage, new MockTimeServiceProvider(), new TimeTrackManagerConfiguration { });
            
            Assert.Null(timeTrackManager.CurrentTrackingActivity);
            
            var command = new ResumeCommand(storage, timeTrackManager);
            command.Execute("Last");

            var current = timeTrackManager.CurrentTrackingActivity;
            Assert.NotNull(current);
            Assert.Equal("1 Activity before", current.Identifier);

        }
        [Fact]
        public void ResumeActivity_CommandLast_WithEmptyStorage_DoesNotStart()
        {
            var storage = new MockDataStorage();

            var timeTrackManager = new TimeTrackManager(storage, new MockTimeServiceProvider(), new TimeTrackManagerConfiguration { });

            var command = new ResumeCommand(storage, timeTrackManager);
            command.Execute("Last");

            var current = timeTrackManager.CurrentTrackingActivity;
            Assert.Null(current);
            Assert.Empty(storage.GetTrackedActivities());

        }

        [Fact]
        public void ResumeActivity_CommandLastWithCurrentTracking_ReturnsLastTrackedActivity()
        {
            var storage = new MockDataStorage(new[] {
                new MockTrackedActivity{ Identifier = "1 Activity before" , Modified = DateTime.Now},
                new MockTrackedActivity{ Identifier = "2 Activity before" , Modified = DateTime.Now.AddMinutes(-1) }

            });
            storage.TrackingActivity = new MockTrackingActivity { Identifier = "To be skipped" };

            var timeTrackManager = new TimeTrackManager(storage, new MockTimeServiceProvider(), new TimeTrackManagerConfiguration { });

            Assert.Equal(2, storage.GetTrackedActivities().Count());
            Assert.NotNull(timeTrackManager.CurrentTrackingActivity);
            Assert.Equal("To be skipped", timeTrackManager.CurrentTrackingActivity.Identifier);

            var command = new ResumeCommand(storage, timeTrackManager);
            command.Execute("Last");

            var current = timeTrackManager.CurrentTrackingActivity;
            Assert.NotNull(current);
            Assert.Equal("1 Activity before", current.Identifier);
            Assert.Equal(3, storage.GetTrackedActivities().Count());
        }
    }

}
