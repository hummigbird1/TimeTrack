namespace TimeTrack.Console.Interfaces
{
    public interface ICustomPlugin
    {
        string Name { get; }

        void Execute(string command);
    }
}