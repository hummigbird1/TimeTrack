namespace TimeTrack.Console.Exceptions
{
    public class InvalidTimeRangeException : ExceptionBase
    {
        public InvalidTimeRangeException(TimeRange range) : base(ExitCodes.InvalidTimeRangeSpecified, $"Invalid Time Range specified! From ({range.From}) should be earlier than to ({range.To}).")
        {
        }
    }
}