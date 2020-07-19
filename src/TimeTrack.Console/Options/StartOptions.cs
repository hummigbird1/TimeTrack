using CommandLine;

namespace TimeTrack.Console.Options
{
    [Verb("start", HelpText = "Starts tracking")]
    public class StartOptions : CommonOptionsBase, ICommandOption
    {
        public Commands Command { get; } = Commands.Start;

        [Option('i', longName: "identifer", HelpText = "The identifier of the activity to start tracking e.g. Ticket number etc.")]
        public string Identifier { get; set; }
    }
}