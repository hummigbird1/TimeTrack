using System;
using System.Linq;
using TimeTrack.Console.Interfaces;
using TimeTrack.Console.Options;

namespace TimeTrack.Console.QueryHandling
{
    internal class AvailableAliasesQuery : IHandler<QueryOptions, string>
    {
        private readonly Configuration.Configuration _configuration;

        public AvailableAliasesQuery(Configuration.Configuration configuration)
        {
            _configuration = configuration;
        }

        public string Handle(QueryOptions argument)
        {
            return string.Join(Environment.NewLine, _configuration.Aliases.Select(x => x.Name).OrderBy(x => x));
        }
    }
}