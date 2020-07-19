using Morestachio;
using Morestachio.Formatter.Framework;
using System;

namespace TimeTrack.Application.Common.TextTemplating
{
    public static class MorestachioExtensionFunctions
    {
        public static ParserOptions AddToOptimizedStringTimespanFormatterFunction(this ParserOptions parserOptions)
        {
            const string FunctionName = nameof(ExtensionMethods.ToOptimizedString);

            parserOptions.Formatters.AddSingle<TimeSpan, string>((timeSpan) => timeSpan.ToOptimizedString(true), FunctionName);
            parserOptions.Formatters.AddSingle<TimeSpan, bool, string>((timeSpan, noDays) => timeSpan.ToOptimizedString(noDays), FunctionName);

            return parserOptions;
        }
    }
}