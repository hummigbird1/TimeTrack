using System;

namespace TimeTrack.Console.Exceptions
{
    public abstract class ExceptionBase : Exception
    {
        public ExceptionBase(ExitCodes exitCode)
        {
            ExitCode = exitCode;
        }

        public ExceptionBase(ExitCodes exitCode, string message) : base(message)
        {
            ExitCode = exitCode;
        }

        public ExceptionBase(ExitCodes exitCode, string message, Exception innerException) : base(message, innerException)
        {
            ExitCode = exitCode;
        }

        public ExitCodes ExitCode { get; }
    }
}