using CommandLine;

namespace TimeTrack.Console.Options
{
    [Verb("export", HelpText = "Exports the recorded data into files")]
    public class ExportOptions : CommonOptionsBase, ICommandOption
    {
        public Commands Command => Commands.Export;

        [Option('t', longName: "export-type", Required = true, HelpText = "e.g. plain, groupedbyidentifier (Try query -t \"avaiable-export-types\" for a complete list)")]
        public string ExportType { get; set; }

        [Option("from", HelpText = "Start date to retrieve entries")]
        public string From { get; set; }

        [Option('o', longName: "output-file-path", Required = true)]
        public string OutputFilePath { get; set; }

        [Option('s', longName: "separator")]
        public string Separator { get; set; }

        [Option("to", HelpText = "End date to retrieve entries")]
        public string To { get; set; }
    }
}