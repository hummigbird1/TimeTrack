using System.Collections.Generic;
using System.Composition;
using TimeTrack.Application.Common.Interfaces;
using TimeTrack.Console.Interfaces;

namespace TimeTrack.Console
{
    internal class Plugins
    {
        [ImportMany]
        public IEnumerable<IDependencyInjectionTypeProvider<ICustomPlugin>> CustomPluginTypes { get; set; }

        [ImportMany]
        public IEnumerable<IDataStorageFactory> DataStorageFactories { get; set; }

        [ImportMany]
        public IEnumerable<IDependencyInjectionTypeProvider<IDateTimeStringParser>> DateTimeStringParserTypes { get; set; }

        [ImportMany]
        public IEnumerable<IDependencyInjectionTypeProvider<ITimeTrackingUpdatedHandler>> TimeTrackingUpdatedHandlerTypes { get; set; }
    }
}