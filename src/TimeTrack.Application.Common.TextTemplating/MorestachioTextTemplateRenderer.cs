using TimeTrack.Application.Common.Interfaces;

namespace TimeTrack.Application.Common.TextTemplating
{
    public class MorestachioTextTemplateRenderer : ITextTemplateRenderer
    {
        public string Render(string template, object model)
        {
            if (string.IsNullOrWhiteSpace(template) || model == null)
            {
                return template;
            }

            return MorestachioTemplateFunctions.RenderTemplate(template, model);
        }
    }
}