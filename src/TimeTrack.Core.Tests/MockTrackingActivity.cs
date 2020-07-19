using System;
using TimeTrack.Interfaces;

namespace TimeTrack.Core.Tests
{
    internal class MockTrackingActivity : ITrackingActivity
    {
        public DateTime Created { get; set; }
        public string Identifier { get; set; }
        public DateTime Modified { get; set; }
        public string RecordId { get; set; }
        public DateTime Started { get; set; }
    }
}