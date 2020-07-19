using System;
using System.Collections.Generic;
using System.Linq;
using TimeTrack.Console.Interfaces;
using TimeTrack.Interfaces;

namespace TimeTrack.Console.Plugin.DateTimeParser
{
    // Replace with a C# Implementation of Approxidate Parser?
    public class DateTimeStringParser : IDateTimeStringParser
    {
        private readonly ITimeServiceProvider _timeServiceProvider;
        private Dictionary<PseudoDates, string[]> _pseudoDateMappings = new Dictionary<PseudoDates, string[]>();
        private Dictionary<PseudoTimes, string[]> _pseudoTimeMappings = new Dictionary<PseudoTimes, string[]>();

        public DateTimeStringParser(ITimeServiceProvider timeServiceProvider)
        {
            _timeServiceProvider = timeServiceProvider ?? throw new ArgumentNullException(nameof(timeServiceProvider));
            _pseudoDateMappings.Add(PseudoDates.Today, new[] { "today", "heute" });
            _pseudoDateMappings.Add(PseudoDates.Yesterday, new[] { "yesterday", "gestern" });

            _pseudoTimeMappings.Add(PseudoTimes.Noon, new[] { "noon", "mittag" });
        }

        private enum PseudoDates
        {
            Today,
            Yesterday,
        }

        private enum PseudoTimes
        {
            Noon
        }

        public string[] CustomFormats { get; set; } = new[] { "yyyyMMdd HH:mm", "yyyyMMdd HH:mm:ss", "yyyyMMdd HHmm", "yyyyMMdd HHmmss", "yyyyMMddTHH:mm", "yyyyMMddTHH:mm:ss", "yyyyMMddTHHmm", "yyyyMMddTHHmmss" };

        public bool TryParse(string dateTimeString, bool isFromDate, out DateTime dateTime)
        {
            return TryParseCore(isFromDate, dateTimeString, out dateTime);
        }

        private DateTime CreateTimeFromParts(PseudoDateResult pseudoParts, DateTime concrete)
        {
            DateTime dateTime = pseudoParts.Date.GetValueOrDefault(concrete).Date;
            TimeSpan tod = concrete.TimeOfDay;
            if (pseudoParts.ExplicitTimeSpecified)
            {
                tod = pseudoParts.Time.Value;
            }

            return dateTime.Add(tod);
        }

        private PseudoDateResult FindPseudoDateTimeStrings(bool isFrom, string dateString, out string remainingDateString)
        {
            remainingDateString = dateString;
            var result = new PseudoDateResult();
            foreach (var d in _pseudoDateMappings)
            {
                foreach (var e in d.Value)
                {
                    var idx = dateString.IndexOf(e, StringComparison.OrdinalIgnoreCase);
                    if (idx > -1)
                    {
                        remainingDateString = remainingDateString.Remove(idx, e.Length);
                        var r = GetPseudoDate(d.Key, isFrom);
                        result.Date = r.Date;
                        result.Time = r.TimeOfDay;
                    }
                }
            }

            foreach (var d in _pseudoTimeMappings)
            {
                foreach (var e in d.Value)
                {
                    var idx = dateString.IndexOf(e, StringComparison.OrdinalIgnoreCase);
                    if (idx > -1)
                    {
                        remainingDateString = remainingDateString.Remove(idx, e.Length);
                        result.Time = GetPseudoTime(d.Key);
                        result.ExplicitTimeSpecified = true;
                    }
                }
            }

            return result;
        }

        private DateTime GetPseudoDate(PseudoDates pseudoDate, bool isFrom)
        {
            switch (pseudoDate)
            {
                case PseudoDates.Today:
                    {
                        var dateTime = _timeServiceProvider.Now().Date;
                        if (!isFrom)
                        {
                            dateTime = dateTime.AddDays(1).AddSeconds(-1);
                        }

                        return dateTime;
                    }
                case PseudoDates.Yesterday:
                    {
                        var dateTime = _timeServiceProvider.Now().Date.AddDays(-1);
                        if (!isFrom)
                        {
                            dateTime = dateTime.AddDays(1).AddSeconds(-1);
                        }
                        return dateTime;
                    }
                default:
                    throw new NotImplementedException($"{pseudoDate} is not implemented!");
            }
        }

        private TimeSpan GetPseudoTime(PseudoTimes pseudoTime)
        {
            switch (pseudoTime)
            {
                case PseudoTimes.Noon:
                    return new TimeSpan(12, 0, 0);

                default:
                    throw new NotImplementedException($"{pseudoTime} is not implemented!");
            }
        }

        private bool TryParseCore(bool isFrom, string dateString, out DateTime dateTime)
        {
            dateTime = DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(dateString))
            {
                return false;
            }

            var pseudoDateTimeValues = FindPseudoDateTimeStrings(isFrom, dateString, out var remainingDateString);
            if (string.IsNullOrWhiteSpace(remainingDateString))
            {
                dateTime = pseudoDateTimeValues.ToDateTime();
                return true;
            }

            if (DateTime.TryParse(remainingDateString, out dateTime))
            {
                dateTime = CreateTimeFromParts(pseudoDateTimeValues, dateTime);
                return true;
            }

            if (CustomFormats != null && CustomFormats.Any())
            {
                foreach (var f in CustomFormats)
                {
                    if (DateTime.TryParseExact(remainingDateString, f, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out dateTime))
                    {
                        dateTime = CreateTimeFromParts(pseudoDateTimeValues, dateTime);
                        return true;
                    }
                }
            }

            return false;
        }

        private struct PseudoDateResult
        {
            public DateTime? Date { get; set; }
            public bool ExplicitTimeSpecified { get; set; }
            public TimeSpan? Time { get; set; }

            public DateTime ToDateTime()
            {
                if (Date.HasValue && Time.HasValue)
                {
                    return Date.Value.Date.Add(Time.Value);
                }
                if (Date.HasValue)
                {
                    return Date.Value.Date;
                }
                else
                {
                    throw new InvalidOperationException("Can not convert to real date time! Missing Date!");
                }
            }
        }
    }
}