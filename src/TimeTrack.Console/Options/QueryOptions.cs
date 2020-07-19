using CommandLine;

namespace TimeTrack.Console.Options
{
    [Verb("query", HelpText = "Query information")]
    public class QueryOptions : CommonOptionsBase, ICommandOption
    {
        public Commands Command { get; } = Commands.Query;

        [Option("from", HelpText = "Start date to retrieve entries")]
        public string From { get; set; }

        [Option('t', longName: "type", HelpText = "Type of information to retrieve (e.g. lastidentifiers, currently tracking activity. Use \"available-queries\" for a complete list)")]
        public string RequestType { get; set; }

        [Option("to", HelpText = "End date to retrieve entries")]
        public string To { get; set; }
    }
}