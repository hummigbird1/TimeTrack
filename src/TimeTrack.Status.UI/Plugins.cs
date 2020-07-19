using System.Collections.Generic;
using System.Composition;
using TimeTrack.Application.Common.Interfaces;

namespace TimeTrack.Status.UI
{
    public class Plugins
    {
        [ImportMany]
        public IEnumerable<IDataStorageFactory> DataStorageFactories { get; set; }
    }
}