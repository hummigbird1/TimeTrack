using System;

namespace TimeTrack.Console.Interfaces
{
    public interface IDateTimeStringParser
    {
        bool TryParse(string dateTimeString, bool isFromDate, out DateTime dateTime);
    }
}