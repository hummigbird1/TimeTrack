using System;

namespace TimeTrack.Core
{
    public class TimeTrackManagerConfiguration
    {
        public TimeSpan? AutoDiscardThreshold { get; set; }
        public bool CaseSensitive { get; set; } = true;
        public bool IgnoreRestartSameActivity { get; set; } = false;
    }
}