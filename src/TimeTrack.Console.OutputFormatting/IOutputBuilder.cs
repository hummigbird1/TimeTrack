using System.Collections.Generic;

namespace TimeTrack.Console.OutputFormatting
{
    public interface IOutputBuilder
    {
        IList<OutputRow> CreateOutputRows<T>(IEnumerable<OutputProperty<T>> outputProperties, IEnumerable<T> data);
    }
}