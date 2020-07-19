namespace TimeTrack.Console.Interfaces
{
    public interface IHandler<TInputArgument, TOutput>
    {
        TOutput Handle(TInputArgument argument);
    }
}