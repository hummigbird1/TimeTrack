using System;
using System.Collections.Generic;
using System.Linq;
using TimeTrack.Console.Interfaces;
using TimeTrack.Console.Options;

namespace TimeTrack.Console.QueryHandling
{
    internal class AvailableCustomPluginsQuery : IHandler<QueryOptions, string>
    {
        private readonly IEnumerable<ICustomPlugin> _customPlugins;

        public AvailableCustomPluginsQuery(IEnumerable<ICustomPlugin> customPlugins)
        {
            _customPlugins = customPlugins;
        }

        public string Handle(QueryOptions argument)
        {
            return string.Join(Environment.NewLine, _customPlugins.Select(x => x.Name).OrderBy(x => x));
        }
    }
}