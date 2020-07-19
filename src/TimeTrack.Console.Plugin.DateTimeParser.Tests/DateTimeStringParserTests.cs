using System;
using TimeTrack.Tests.Common;
using Xunit;

namespace TimeTrack.Console.Plugin.DateTimeParser.Tests
{
    public class DateTimeStringParserTests
    {
        [Fact]
        public void Mixed()
        {
            var timeService = new MockTimeServiceProvider();
            var parser = new DateTimeStringParser(timeService);
            var result = parser.TryParse("Today 10:12", true, out var resultTime);
            Assert.True(result);

            Assert.Equal(DateTime.Now.Date, resultTime.Date);
            Assert.Equal(new TimeSpan(10, 12, 0), resultTime.TimeOfDay);
        }

        [Fact]
        public void Normal_From()
        {
            var timeService = new MockTimeServiceProvider();
            var parser = new DateTimeStringParser(timeService);
            var result = parser.TryParse("today", true, out var resultTime);
            Assert.True(result);

            Assert.Equal(DateTime.Now.Date, resultTime.Date);
            Assert.Equal(new TimeSpan(0, 0, 0), resultTime.TimeOfDay);
        }

        [Fact]
        public void Normal_To()
        {
            var timeService = new MockTimeServiceProvider();
            var parser = new DateTimeStringParser(timeService);
            var result = parser.TryParse("today", false, out var resultTime);
            Assert.True(result);

            Assert.Equal(DateTime.Now.Date, resultTime.Date);
            Assert.Equal(new TimeSpan(23, 59, 59), resultTime.TimeOfDay);
        }

        [Fact]
        public void Normal2()
        {
            var timeService = new MockTimeServiceProvider();
            var parser = new DateTimeStringParser(timeService);
            var result = parser.TryParse("03.05.2020", true, out var resultTime);
            Assert.True(result);

            Assert.Equal(new DateTime(2020, 5, 3), resultTime.Date);
            Assert.Equal(new TimeSpan(0, 0, 0), resultTime.TimeOfDay);
        }

        [Fact]
        public void Normal3()
        {
            var timeService = new MockTimeServiceProvider();
            var parser = new DateTimeStringParser(timeService);
            var result = parser.TryParse("12.06.2020 21:56", true, out var resultTime);
            Assert.True(result);

            Assert.Equal(new DateTime(2020, 6, 12), resultTime.Date);
            Assert.Equal(new TimeSpan(21, 56, 0), resultTime.TimeOfDay);
        }
    }
}