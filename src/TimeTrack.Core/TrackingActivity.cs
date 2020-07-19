using System;
using TimeTrack.Interfaces;

namespace TimeTrack.Core
{
    internal class TrackingActivity : ITrackingActivity
    {
        public DateTime Created { get; set; }
        public string Identifier { get; set; }
        public DateTime Modified { get; set; }
        public string RecordId { get; } = null;
        public DateTime Started { get; set; }
    }
}