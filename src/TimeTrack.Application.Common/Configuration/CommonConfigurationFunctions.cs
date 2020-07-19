using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace TimeTrack.Application.Common.Configuration
{
    public static class CommonConfigurationFunctions
    {
        public static IConfigurationRoot CreateConfigurationFromJsonFiles(string configurationPath, out string finalFilePath, string configurationFileSearchPattern = "*.json")
        {
            if (configurationPath == null)
            {
                throw new ArgumentNullException(nameof(configurationPath));
            }

            finalFilePath = Environment.ExpandEnvironmentVariables(configurationPath);
            var configBuilder = new ConfigurationBuilder();
            foreach (var f in Directory.GetFiles(finalFilePath, configurationFileSearchPattern))
            {
                configBuilder.AddJsonFile(f, false, true);
            }
            return configBuilder.Build();
        }

        public static string GetDefaultConfigurationPath(string environmentVariableName = "TimeTrack.Application.ConfigurationPath", string applicationSubfolderName = "Configuration")
        {
            var configFilePathFromEnvVar = Environment.GetEnvironmentVariable(environmentVariableName);
            if (!string.IsNullOrWhiteSpace(configFilePathFromEnvVar))
            {
                return configFilePathFromEnvVar;
            }
            else
            {
                var exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var defaultConfigurationDirectoryPath = Path.Combine(exePath, applicationSubfolderName);
                if (Directory.Exists(defaultConfigurationDirectoryPath))
                {
                    return defaultConfigurationDirectoryPath;
                }
            }

            return null;
        }
    }
}