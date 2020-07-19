namespace TimeTrack.Application.Common.Interfaces
{
    public interface ITextTemplateRenderer
    {
        string Render(string template, object model);
    }
}
