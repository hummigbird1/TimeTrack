using System;
using TimeTrack.Interfaces;

namespace TimeTrack.LiteDb
{
    public class TrackingActivity : ITrackingActivity
    {
        public DateTime Created { get; set; }
        public string Identifier { get; set; }
        public DateTime Modified { get; set; }
        public string RecordId { get; }
        public DateTime Started { get; set; }
    }
}