using System;
using System.Linq;
using TimeTrack.Console.Export;
using TimeTrack.Console.Interfaces;
using TimeTrack.Console.Options;

namespace TimeTrack.Console.QueryHandling
{
    internal class AvailableExportTypesQuery : IHandler<QueryOptions, string>
    {
        private readonly ISelectorTypeRegistrationCatalog<string, IExporter> _exportTypeSelectorTypeRegistrationCatalog;

        public AvailableExportTypesQuery(ISelectorTypeRegistrationCatalog<string, IExporter> listTypeSelectorTypeRegistrationCatalog)
        {
            _exportTypeSelectorTypeRegistrationCatalog = listTypeSelectorTypeRegistrationCatalog;
        }

        public string Handle(QueryOptions argument)
        {
            return string.Join(Environment.NewLine, _exportTypeSelectorTypeRegistrationCatalog.Keys.OrderBy(x => x));
        }
    }
}