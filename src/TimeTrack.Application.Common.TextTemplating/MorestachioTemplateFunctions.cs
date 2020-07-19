namespace TimeTrack.Application.Common.TextTemplating
{
    public static class MorestachioTemplateFunctions
    {
        public static Morestachio.MorestachioDocumentInfo Create(string template)
        {
            var parserOptions = new Morestachio.ParserOptions(template);

            parserOptions.AddToOptimizedStringTimespanFormatterFunction();

            return Morestachio.Parser.ParseWithOptions(parserOptions);
        }

        public static string RenderTemplate(string template, object model)
        {
            return Create(template).CreateAndStringify(model);
        }
    }
}