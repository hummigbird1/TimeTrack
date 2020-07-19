using System;
using TimeTrack.Application.Common.Interfaces;
using TimeTrack.Console.Exceptions;
using TimeTrack.Console.Interfaces;

namespace TimeTrack.Console
{
    public class TimeRange
    {
        private readonly IDateTimeStringParser _dateTimeStringParser;

        public TimeRange(IServiceTypeSelector<IDateTimeStringParser> dateTimeStringParser) : this(dateTimeStringParser?.GetRequired())
        {
        }

        private TimeRange(IDateTimeStringParser dateTimeStringParser)
        {
            _dateTimeStringParser = dateTimeStringParser ?? throw new ArgumentNullException(nameof(dateTimeStringParser));
        }

        public DateTime From { get; set; } = DateTime.MinValue;

        public bool IsValidRange
        {
            get
            {
                return From < To;
            }
        }

        public DateTime To { get; set; } = DateTime.MaxValue;

        public static TimeRange Create(IDateTimeStringParser dateTimeStringParser)
        {
            return new TimeRange(dateTimeStringParser);
        }

        public void ParseFrom(string dateTimeString, DateTime defaultDateTime)
        {
            if (!TryParseFrom(dateTimeString))
            {
                From = defaultDateTime;
            }
        }

        public void ParseTo(string dateTimeString, DateTime defaultDateTime)
        {
            if (!TryParseTo(dateTimeString))
            {
                To = defaultDateTime;
            }
        }

        public void ThrowExceptionIfInvalid()
        {
            if (!IsValidRange)
            {
                throw new InvalidTimeRangeException(this);
            }
        }

        public bool TryParseFrom(string dateTimeString)
        {
            if (_dateTimeStringParser.TryParse(dateTimeString, true, out var newFrom))
            {
                From = newFrom;
                return true;
            }

            return false;
        }

        public bool TryParseTo(string dateTimeString)
        {
            if (_dateTimeStringParser.TryParse(dateTimeString, false, out var newTo))
            {
                To = newTo;
                return true;
            }

            return false;
        }
    }
}