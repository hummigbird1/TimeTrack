using System;
using TimeTrack.Application.Plugin.LiteDbStorage.Configuration;

namespace TimeTrack.Application.Plugin.LiteDbStorage
{
    internal static class LiteDbConnectionStringBuilder
    {
        public static string BuildFromConfiguration(LiteDbDataStoragePluginConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrWhiteSpace(configuration.FileName))
            {
                throw new InvalidOperationException("No file name specified!");
            }

            var mode = "Shared"; // Or 'Direct' for exclusive mode
            if (!string.IsNullOrWhiteSpace(configuration.Mode))
            {
                mode = configuration.Mode;
            }

            return $"FileName={Environment.ExpandEnvironmentVariables(configuration.FileName)};Connection={mode}";
        }
    }
}