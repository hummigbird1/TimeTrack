using System;
using TimeTrack.Interfaces;

namespace TimeTrack.Tests.Common
{
    public class MockTrackedActivity : ITrackedActivity
    {
        public DateTime Created { get; set; }
        public string Identifier { get; set; }

        public DateTime Modified { get; set; }
        public string RecordId { get; }

        public DateTime Started { get; set; }

        public DateTime Stopped { get; set; }
    }
}