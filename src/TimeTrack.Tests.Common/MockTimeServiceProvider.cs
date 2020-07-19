using System;
using TimeTrack.Interfaces;

namespace TimeTrack.Tests.Common
{
    public class MockTimeServiceProvider : ITimeServiceProvider
    {
        private readonly DateTime _nowTime;

        public MockTimeServiceProvider() : this(DateTime.Now)
        {
        }

        public MockTimeServiceProvider(DateTime nowTime)
        {
            _nowTime = nowTime;
        }

        public DateTime Now()
        {
            return _nowTime;
        }
    }
}