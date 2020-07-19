using TimeTrack.Console.Interfaces;

namespace TimeTrack.Console
{
    public class OutputLogger : ILogger
    {
        private readonly IOutputWriter _outputWriter;

        public OutputLogger(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public bool Enabled { get; set; }

        public void Verbose(string s)
        {
            if (!Enabled)
            {
                return;
            }

            _outputWriter.WriteLine(s);
        }
    }
}