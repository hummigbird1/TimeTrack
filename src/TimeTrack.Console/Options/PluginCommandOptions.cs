using CommandLine;

namespace TimeTrack.Console.Options
{
    [Verb("custom-plugin")]
    public class PluginCommandOptions : CommonOptionsBase, ICommandOption
    {
        [Option('d', "command", HelpText = "The optional plugin specific command to execute (if the plugin provides that)")]
        public string Command { get; set; }

        Commands ICommandOption.Command => Commands.PluginCommand;

        [Option('n', "name", Required = true, HelpText = "The plugin to execute. (Try query -t \"avaiable-custom-plugins\" for a complete list)")]
        public string Name { get; set; }
    }
}