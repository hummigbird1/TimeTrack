using CommandLine;

namespace TimeTrack.Console.Options
{
    [Verb("list", HelpText = "Prints the recorded data to the standard output")]
    public class ListOptions : CommonOptionsBase, ICommandOption
    {
        public Commands Command { get; } = Commands.List;

        [Option("from", HelpText = "Start date to retrieve entries")]
        public string From { get; set; }

        [Option("sort-desc", HelpText = "Output records in Descending Order")]
        public bool SortDescending { get; set; }

        [Option("sort-by", HelpText = "Property to sort records by")]
        public string SortProperty { get; set; }

        [Option('s', longName: "summarize", HelpText = "Summarizes all entries and groups them by identifer")]
        public bool Summarize { get; set; }

        [Option("to", HelpText = "End date to retrieve entries")]
        public string To { get; set; }

        public Sorting GetSorting()
        {
            return new Sorting
            {
                SortByPropertyName = SortProperty,
                SortDescending = SortDescending
            };
        }
    }
}