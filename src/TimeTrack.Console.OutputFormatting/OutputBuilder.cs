using System.Collections.Generic;
using System.Linq;

namespace TimeTrack.Console.OutputFormatting
{
    public class OutputBuilder : IOutputBuilder
    {
        public IList<OutputRow> CreateOutputRows<T>(IEnumerable<OutputProperty<T>> outputProperties, IEnumerable<T> outputData)
        {
            var outputPropertyList = outputProperties.ToList();
            var headerRow = CreateHeaderRow(outputPropertyList);
            var dataRows = new[] { headerRow }
                .Union(outputData.Select(x => CreateDataRow(outputPropertyList, x)))
                .ToList();
            SetRowPadding(dataRows);
            return dataRows;
        }

        private OutputRow CreateDataRow<T>(IList<OutputProperty<T>> outputProperties, T entry)
        {
            return new OutputRow
            {
                ColumnValues = outputProperties.Select(x => new PaddableColumn
                {
                    Value = x.ValueConversionFunction(entry),
                    PadLeft = x.RightAligned
                }).ToArray()
            };
        }

        private OutputRow CreateHeaderRow<T>(IList<OutputProperty<T>> outputProperties)
        {
            return new OutputRow
            {
                ColumnValues = outputProperties.Select(x => new PaddableColumn
                {
                    Value = x.HeaderText,
                    PadLeft = x.RightAligned,
                }).ToArray()
            };
        }

        private void SetRowPadding(List<OutputRow> rows)
        {
            var columns = rows.First().ColumnValues;
            for (var x = 0; x < columns.Length; x++)
            {
                var maxWidthRequired = rows.Max(r => r.ColumnValues[x].TotalLength);
                rows.ForEach(r => r.ColumnValues[x].PaddingTotalWith = maxWidthRequired);
            }
        }
    }
}