namespace TimeTrack.Console.Interfaces
{
    public interface ISelectorTypeServiceProvider<TSelector, TInstance>
    {
        TInstance this[TSelector key] { get; }
    }
}