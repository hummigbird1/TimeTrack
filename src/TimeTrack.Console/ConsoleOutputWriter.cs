using TimeTrack.Console.Interfaces;

namespace TimeTrack.Console
{
    public class ConsoleOutputWriter : IOutputWriter
    {
        public void WriteLine(string s)
        {
            System.Console.WriteLine(s);
        }
    }
}