using System;
using System.Collections.Generic;
using System.Composition;
using TimeTrack.Application.Common.Interfaces;
using TimeTrack.Console.Interfaces;

namespace TimeTrack.Console.Plugin.DateTimeParser
{
    [Export(typeof(IDependencyInjectionTypeProvider<IDateTimeStringParser>))]
    public class DateTimeStringParserTypeProvider : IDependencyInjectionTypeProvider<IDateTimeStringParser>
    {
        public IEnumerable<Type> TransientTypes => new[] {
            typeof(ChronicDateTimeStringParser),
            typeof(DateTimeStringParser)
        };
    }
}