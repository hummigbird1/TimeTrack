using System;

namespace TimeTrack.Interfaces
{
    public interface IStorageRecord
    {
        DateTime Created { get; }
        DateTime Modified { get; set; }
        string RecordId { get; }
    }
}