using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using TimeTrack.Console.Interfaces;
using TimeTrack.Console.ListHandling;
using TimeTrack.Console.Options;
using TimeTrack.Console.OutputFormatting;
using TimeTrack.Application.Common;
using TimeTrack.Application.Common.Interfaces;

namespace TimeTrack.Console.CommandHandlers
{
    internal class ListCommandHandler : IHandler<ICommandOption, int>
    {
        private readonly IDateTimeStringParser _dateTimeStringParser;
        private readonly IDynamicPropertySorter _dynamicPropertySorter;
        private readonly IOutputBuilder _outputBuilder;
        private readonly IOutputPropertyFactory _outputPropertyFactory;
        private readonly IOutputWriter _outputWriter;
        private readonly IActivityQueryProvider<PlainListItem> _plainItemQueryProvider;
        private readonly IActivityQueryProvider<SummaryListItem> _summaryItemQueryProvider;

        public ListCommandHandler(
            IServiceTypeSelector<IDateTimeStringParser> dateTimeStringParser,
            IActivityQueryProvider<SummaryListItem> summaryItemQueryProvider,
            IActivityQueryProvider<PlainListItem> plainItemQueryProvider,
            IDynamicPropertySorter dynamicPropertySorter,
            IOutputPropertyFactory outputPropertyFactory,
            IOutputBuilder outputBuilder,
            IOutputWriter outputWriter
            )
        {
            _dateTimeStringParser = dateTimeStringParser.GetRequired();
            _summaryItemQueryProvider = summaryItemQueryProvider;
            _plainItemQueryProvider = plainItemQueryProvider;
            _dynamicPropertySorter = dynamicPropertySorter;
            _outputPropertyFactory = outputPropertyFactory;
            _outputBuilder = outputBuilder;
            _outputWriter = outputWriter;
        }

        public int Handle(ICommandOption argument)
        {
            ExecuteCommandCore((ListOptions)argument);
            return (int)ExitCodes.Successful;
        }

        private void ExecuteCommandCore(ListOptions argument)
        {
            var range = TimeRange.Create(_dateTimeStringParser);
            range.ParseFrom(argument.From, DateTime.MinValue);
            range.ParseTo(argument.To, DateTime.MaxValue);
            range.ThrowExceptionIfInvalid();

            var sorting = argument.GetSorting();
            if (argument.Summarize)
            {
                WriteSummarizedOutput(range, sorting);
            }
            else
            {
                WritePlainItemList(range, sorting);
            }

            if (range.From != DateTime.MinValue || range.To != DateTime.MaxValue)
            {
                _outputWriter.WriteLine($"Date Range Filter: {range.From} - {range.To}");
            }
            else
            {
                _outputWriter.WriteLine($"No Date Range Filter");
            }
        }

        private IEnumerable<T> Filter<T, TFilter>(IEnumerable<T> available, IEnumerable<TFilter> selection, Func<T, TFilter, bool> matchPredicate)
        {
            return selection.Select(se => available.Single(av => matchPredicate(av, se)));
        }

        private IEnumerable<OutputProperty<PlainListItem>> GetAvailablePlainListProperties()
        {
            return _outputPropertyFactory.BuildOutputProperties(new Expression<Func<PlainListItem, string>>[] {
                x => x.RecordId,
                x => x.Created.ToString(),
                x => x.Modified.ToString(),
                x => x.IsCurrentlyTracking ? $" => {x.Identifier} <=" : x.Identifier,
                x => x.Started.ToString(),
                x => x.Stopped.ToString(),
                x => x.Duration.ToOptimizedString(true),
            });
        }

        private IEnumerable<OutputProperty<SummaryListItem>> GetAvailableSummarizedListProperties()
        {
            return _outputPropertyFactory.BuildOutputProperties(new Expression<Func<SummaryListItem, string>>[]
            {
                x => x.ContainsCurrentlyTracking ? $" => {x.Identifier} <=" : x.Identifier,
                x => x.RecordCount.ToString(),
                x => x.TotalDuration.ToOptimizedString(true),
                x => x.RecordCount > 1 ? x.AverageDuration.ToOptimizedString(true) : null,
                x => x.TotalDurationJira,
                x => x.From.ToString(),
                x => x.To.ToString()
            });
        }

        private IList<T> GetSortedOutputData<T>(IActivityQueryProvider<T> activityQueryProvider, TimeRange range, Sorting sorting, string defaultSortPropertyName)
        {
            if (string.IsNullOrEmpty(sorting.SortByPropertyName))
            {
                sorting.SortByPropertyName = defaultSortPropertyName;
            }

            return _dynamicPropertySorter.Sort(activityQueryProvider.GetAllActivitiesInTimerange(range.From, range.To), sorting).ToList();
        }

        private IList<OutputProperty<T>> PrepareOutputProperties<T>(IList<OutputProperty<T>> allProperties, string[] columnsToOutput)
        {
            if (columnsToOutput == null || !columnsToOutput.Any())
            {
                return allProperties;
            }

            return Filter(allProperties, columnsToOutput, (e, f) => string.Equals(e.PropertyName, f, StringComparison.OrdinalIgnoreCase) || string.Equals(e.HeaderText, f, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        private void WriteOutput<T>(IList<T> entries, IList<OutputProperty<T>> columnsForOutput, Func<int, IEnumerable<string>> footerLineFactory = null)
        {
            var outputRows = _outputBuilder.CreateOutputRows(columnsForOutput, entries);
            var totalLineLength = WriteOutput(outputRows, "   ");
            if (footerLineFactory != null)
            {
                foreach (var l in footerLineFactory(totalLineLength))
                {
                    _outputWriter.WriteLine(l);
                }
            }
        }

        private int WriteOutput(IList<OutputRow> outputRows, string columnSeparator, char headerSeparator = '_', char footerSeparator = '=')
        {
            var outputColumnCount = outputRows.First().ColumnValues.Count();

            var totalLineLength = (outputColumnCount - 1) * columnSeparator.Length + outputRows.Max(x => x.TotalLength);

            var formatString = string.Join(columnSeparator, Enumerable.Range(0, outputColumnCount).Select(x => $"{{{x}}}"));

            _outputWriter.WriteLine(string.Format(formatString, outputRows[0].ColumnValues));
            _outputWriter.WriteLine("".PadRight(totalLineLength, headerSeparator));

            foreach (var e in outputRows.Skip(1))
            {
                _outputWriter.WriteLine(string.Format(formatString, e.ColumnValues));
            }

            _outputWriter.WriteLine("".PadRight(totalLineLength, footerSeparator));
            return totalLineLength;
        }

        private void WritePlainItemList(TimeRange range, Sorting sorting)
        {
            var defaultOutputColumns = new[]
            {
                nameof(PlainListItem.RecordId),
                nameof(PlainListItem.Identifier),
                nameof(PlainListItem.Started),
                nameof(PlainListItem.Stopped),
                nameof(PlainListItem.Duration)
            };

            var availableOutputProperties = GetAvailablePlainListProperties().ToList();
            var entries = GetSortedOutputData(_plainItemQueryProvider, range, sorting, nameof(PlainListItem.Modified));
            var propertiesToOutput = PrepareOutputProperties(availableOutputProperties, defaultOutputColumns);

            var totalTime = TimeSpan.FromTicks(entries.Sum(x => x.Duration.Ticks));
            WriteOutput(entries, propertiesToOutput, ll => new[] { $"{entries.Count} ROWS => TOTAL: {totalTime.ToOptimizedString(true)}" });
        }

        private void WriteSummarizedOutput(TimeRange range, Sorting sorting)
        {
            var availableOutputProperties = GetAvailableSummarizedListProperties().ToList();
            var entries = GetSortedOutputData(_summaryItemQueryProvider, range, sorting, nameof(SummaryListItem.TotalDuration));
            var propertiesToOutput = PrepareOutputProperties(availableOutputProperties, null);

            var totalTime = TimeSpan.FromTicks(entries.Sum(x => x.TotalDuration.Ticks));
            WriteOutput(entries, propertiesToOutput, ll => new[] { $"{entries.Sum(x => x.RecordCount)} ROWS => {entries.Count()} ACTIVITIES => TOTAL: {totalTime.ToOptimizedString(true)}" });
        }
    }
}