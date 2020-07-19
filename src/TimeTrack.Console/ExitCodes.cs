namespace TimeTrack.Console
{
    public enum ExitCodes
    {
        Successful = 0,
        PluginError = 50,
        ParameterParsingError = 100,
        NoConfigurationSpecified = 110,
        InvalidTimeRangeSpecified = 120,
        UnhandledException = 1000,
        Undefined = 9999,
        CommandSpecificBaseCode = 10000,
    }
}