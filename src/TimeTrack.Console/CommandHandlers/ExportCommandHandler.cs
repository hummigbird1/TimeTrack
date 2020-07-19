using System;
using System.IO;
using TimeTrack.Application.Common.Interfaces;
using TimeTrack.Console.Export;
using TimeTrack.Console.Interfaces;
using TimeTrack.Console.Options;

namespace TimeTrack.Console.CommandHandlers
{
    internal class ExportCommandHandler : IHandler<ICommandOption, int>
    {
        private readonly IDateTimeStringParser _dateTimeStringParser;
        private readonly ISelectorTypeServiceProvider<string, IExporter> _selectorTypeServiceProvider;

        public ExportCommandHandler(IServiceTypeSelector<IDateTimeStringParser> dateTimeStringParser,
            ISelectorTypeServiceProvider<string, IExporter> selectorTypeServiceProvider)
        {
            _dateTimeStringParser = dateTimeStringParser.GetRequired();
            _selectorTypeServiceProvider = selectorTypeServiceProvider;
        }

        public int Handle(ICommandOption argument)
        {
            ExportDataToFile((ExportOptions)argument);
            return (int)ExitCodes.Successful;
        }

        private void ExportDataToFile(ExportOptions options)
        {
            var range = TimeRange.Create(_dateTimeStringParser);
            range.ParseFrom(options.From, DateTime.MinValue);
            range.ParseTo(options.To, DateTime.MaxValue);
            range.ThrowExceptionIfInvalid();

            var separator = "\t";
            if (!string.IsNullOrWhiteSpace(options.Separator))
            {
                separator = options.Separator;
            }

            var filePath = options.OutputFilePath;
            bool fileExists = File.Exists(filePath);

            var exporter = _selectorTypeServiceProvider[options.ExportType.ToLowerInvariant()];

            using (var writeStream = File.OpenWrite(filePath))
            {
                if (writeStream.Length > 0)
                {
                    writeStream.Position = writeStream.Length - 1;
                }
                exporter.Export(!fileExists, separator, range.From, range.To, writeStream);
            }
        }
    }
}