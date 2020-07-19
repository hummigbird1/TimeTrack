using System;
using TimeTrack.Console.Interfaces;

namespace TimeTrack.Console.Plugin.DateTimeParser
{
    internal class ChronicDateTimeStringParser : IDateTimeStringParser
    {
        public bool TryParse(string dateTimeString, bool isFromDate, out DateTime dateTime)
        {
            dateTime = DateTime.MinValue;
            if (string.IsNullOrWhiteSpace(dateTimeString))
            {
                return false;
            }

            if (!TryParse(dateTimeString, out var parseResult))
            {
                return false;
            }

            var parsedValue = isFromDate ? parseResult.Start : parseResult.End;

            if (!parsedValue.HasValue)
            {
                return false;
            }

            dateTime = parsedValue.Value;
            return true;
        }

        private bool TryParse(string dateTimeString, out ChronicNetCore.Span result)
        {
            try
            {
                var parser = new ChronicNetCore.Parser();
                result = parser.Parse(dateTimeString);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
}