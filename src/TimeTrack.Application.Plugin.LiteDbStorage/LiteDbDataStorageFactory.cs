using Microsoft.Extensions.Configuration;
using System;
using System.Composition;
using TimeTrack.Application.Common;
using TimeTrack.Application.Common.Interfaces;
using TimeTrack.Application.Plugin.LiteDbStorage.Configuration;
using TimeTrack.Interfaces;
using TimeTrack.LiteDb;

namespace TimeTrack.Application.Plugin.LiteDbStorage
{
    [Export(typeof(IDataStorageFactory))]
    public class LiteDbDataStorageFactory : IDataStorageFactory
    {
        public IDataStorage CreateDataStorageInstance(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return new LiteDbDataStorage(BuildConnectionStringFromConfiguration(configuration));
        }

        private string BuildConnectionStringFromConfiguration(IConfiguration configuration)
        {
            var configSectionName = nameof(LiteDbDataStorageFactory);

            var typedConfiguration = configuration.BindConfigurationSection<LiteDbDataStoragePluginConfiguration>(configSectionName);
            return LiteDbConnectionStringBuilder.BuildFromConfiguration(typedConfiguration);
        }
    }
}