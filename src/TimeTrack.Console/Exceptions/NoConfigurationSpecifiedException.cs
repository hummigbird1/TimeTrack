namespace TimeTrack.Console.Exceptions
{
    public class NoConfigurationSpecifiedException : ExceptionBase
    {
        public NoConfigurationSpecifiedException() : base(ExitCodes.NoConfigurationSpecified, "No configuration was specified!")
        {
        }
    }
}