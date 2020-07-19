using CommandLine;

namespace TimeTrack.Status.UI
{
    public class Options
    {
        [Option('c', "config", HelpText = "GLOBAL: The path where configuration files to use are located")]
        public string ConfigurationPath { get; set; }
    }
}