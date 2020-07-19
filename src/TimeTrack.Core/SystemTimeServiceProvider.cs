using System;
using TimeTrack.Interfaces;

namespace TimeTrack.Core
{
    public class SystemTimeServiceProvider : ITimeServiceProvider
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}