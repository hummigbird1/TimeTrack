using System.Linq;

namespace TimeTrack.Console.OutputFormatting
{
    public class OutputRow
    {
        public PaddableColumn[] ColumnValues { get; set; }

        public int TotalLength => ColumnValues.Sum(x => x.TotalLength);
    }
}