using CommandLine;

namespace TimeTrack.Console.Options
{
    [Verb("run", HelpText = "Executes a predefined alias")]
    public class AliasOptions : CommonOptionsBase, ICommandOption
    {
        [Option('a', longName: "alias", Required = true, HelpText = "The name of the (configured) alias to run. (Try query -t \"avaiable-aliases\" for a complete list)")]
        public string Alias { get; set; }

        public Commands Command => Commands.Alias;
    }
}