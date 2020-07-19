using Microsoft.Extensions.Configuration;

namespace TimeTrack.Console.Configuration
{
    public interface IConfigurationManager
    {
        Configuration Configuration { get; }
        string ConfigurationPath { get; }

        IConfiguration ConfigurationRoot { get; }

        void SetConfigurationFilePath(string configurationFilePath);
    }
}