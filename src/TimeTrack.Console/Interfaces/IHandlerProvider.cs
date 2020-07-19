namespace TimeTrack.Console.Interfaces
{
    public interface IHandlerProvider<TSelector, TInput, TOutput>
    {
        IHandler<TInput, TOutput> GetTypeHandler(TSelector input);
    }

    public interface IHandlerProvider<TSelector, TOutput>
    {
        TOutput GetTypeHandler(TSelector input);
    }
}