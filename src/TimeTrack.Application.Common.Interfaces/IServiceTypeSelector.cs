namespace TimeTrack.Application.Common.Interfaces
{
    public interface IServiceTypeSelector<T>
    {
        T GetRequired();
    }
}