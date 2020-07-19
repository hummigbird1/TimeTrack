using System;
using System.Linq;
using TimeTrack.Console.Interfaces;
using TimeTrack.Console.Options;

namespace TimeTrack.Console.QueryHandling
{
    internal class AvailableQueryTypesQuery : IHandler<QueryOptions, string>
    {
        private readonly ISelectorTypeRegistrationCatalog<string, IHandler<QueryOptions, string>> _selectorTypeRegistrationCatalog;

        public AvailableQueryTypesQuery(ISelectorTypeRegistrationCatalog<string, IHandler<QueryOptions, string>> selectorTypeRegistrationCatalog)
        {
            _selectorTypeRegistrationCatalog = selectorTypeRegistrationCatalog;
        }

        public string Handle(QueryOptions argument)
        {
            return string.Join(Environment.NewLine, _selectorTypeRegistrationCatalog.Keys.OrderBy(x => x));
        }
    }
}