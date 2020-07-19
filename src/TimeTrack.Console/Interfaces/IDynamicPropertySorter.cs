using System.Collections.Generic;
using System.Linq;

namespace TimeTrack.Console.Interfaces
{
    public interface IDynamicPropertySorter
    {
        IOrderedEnumerable<TInput> Sort<TInput>(IEnumerable<TInput> enumerable, Sorting sorting);
    }
}