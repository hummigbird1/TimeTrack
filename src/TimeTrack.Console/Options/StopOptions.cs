using CommandLine;

namespace TimeTrack.Console.Options
{
    [Verb("stop", HelpText = "Stops tracking")]
    public class StopOptions : CommonOptionsBase, ICommandOption
    {
        public Commands Command { get; } = Commands.Stop;

        [Option('d', longName: "discard", HelpText = "Discard the currently tracking activity instead of creating a tracked activity!")]
        public bool Discard { get; set; }
    }
}