using CommandLine;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TimeTrack.Console.Options
{
    public class CommonOptionsBase
    {
        [Option('c', "config", HelpText = "GLOBAL: The path where configuration files to use are located")]
        public string ConfigurationPath { get; set; }

        [Option("confirm", HelpText = "The program does not exit without user confirmation.")]
        public bool ConfirmExit { get; set; }

        [Option('v', "verbose", HelpText = "GLOBAL: Verbose")]
        public bool Verbose { get; set; }

        public override string ToString()
        {
            return GetParameterValues();
        }

        private string GetParameterValues()
        {
            var s = new StringBuilder();
            foreach (var p in GetType().GetProperties().Where(x => x.CanRead && x.CanWrite && x.GetCustomAttribute<OptionAttribute>() != null))
            {
                var value = p.GetValue(this);
                s.AppendFormat("{0}='{1}'", p.Name, value);
                s.Append("   ");
            }
            return s.ToString();
        }
    }
}