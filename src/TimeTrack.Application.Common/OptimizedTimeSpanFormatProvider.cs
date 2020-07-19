using System;

namespace TimeTrack.Application.Common
{
    public class OptimizedTimeSpanFormatProvider : IFormatProvider, ICustomFormatter
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            // Check whether this is an appropriate callback
            if (!this.Equals(formatProvider))
                return null;

            if (arg == null)
                return null;

            switch (format)
            {
                case "NoDays":
                    if (arg is TimeSpan timeSpan)
                    {
                        var h = (int)timeSpan.TotalHours;
                        return string.Format("{0:00}:{1:mm\\:ss}", h, timeSpan);
                    }
                    else
                    {
                        throw new FormatException($"The format '{format}' can not be applied to values of type {arg.GetType()}!");
                    }
                case null:
                case "":
                    if (arg is TimeSpan timeSpan1)
                    {
                        return string.Format(@"{0:d\.hh\:mm\:ss}", timeSpan1);
                    }
                    break;
            }

            if (format != null)
            {
                return string.Format("{0:" + format + "}", arg);
            }

            return string.Format("{0}", arg);
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }
    }
}