namespace TimeTrack.Console.Interfaces
{
    public interface ILogger
    {
        bool Enabled { get; set; }

        void Verbose(string s);
    }
}