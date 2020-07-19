using System;
using System.Collections.Generic;

namespace TimeTrack.Console.Interfaces
{
    public interface IActivityQueryProvider<TOutput>
    {
        IEnumerable<TOutput> GetAllActivitiesInTimerange(DateTime from, DateTime to);
    }
}