namespace TimeTrack.Console.Options
{
    public interface ICommandOption
    {
        Commands Command { get; }

        string ConfigurationPath { get; }

        bool ConfirmExit {get; }
        bool Verbose { get; }
    }
}