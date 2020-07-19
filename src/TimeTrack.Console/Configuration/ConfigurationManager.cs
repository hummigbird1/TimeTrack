using Microsoft.Extensions.Configuration;
using TimeTrack.Application.Common.Configuration;

namespace TimeTrack.Console.Configuration
{
    public class ConfigurationManager : IConfigurationManager
    {
        public Configuration Configuration { get; private set; }

        public string ConfigurationPath { get; private set; }

        public IConfiguration ConfigurationRoot { get; private set; }

        public void SetConfigurationFilePath(string configurationPath)
        {
            var configurationRoot = CommonConfigurationFunctions.CreateConfigurationFromJsonFiles(configurationPath, out var finalConfigurationPath);
            ConfigurationRoot = configurationRoot;
            Configuration = BindConfiguration(configurationRoot);
            ConfigurationPath = finalConfigurationPath;
        }

        private Configuration BindConfiguration(IConfigurationRoot configurationRoot)
        {
            var configuration = new Configuration();
            configurationRoot.Bind(configuration);
            return configuration;
        }
    }
}