using TimeTrack.Application.Common.Configuration;

namespace TimeTrack.Status.UI.Configuration
{
    internal class Configuration
    {
        public DependencyInjectionServiceSelectionDefinition[] DependencyInjectionServiceSelection { get; set; }

        public ApplicationSettings ApplicationSettings { get; set; } = new ApplicationSettings();
    }
}
