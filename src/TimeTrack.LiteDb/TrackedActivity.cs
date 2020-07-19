using LiteDB;
using System;
using TimeTrack.Interfaces;

namespace TimeTrack.LiteDb
{
    public class TrackedActivity : ITrackedActivity
    {
        public TrackedActivity(string identifier, DateTime started, DateTime stopped)
        {
            Identifier = identifier;
            Started = started;
            Stopped = stopped;
        }

        public DateTime Created { get; set; }

        [BsonId]
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Identifier { get; }
        public DateTime Modified { get; set; }
        public string RecordId => Id.ToString();
        public DateTime Started { get; }
        public DateTime Stopped { get; }
    }
}