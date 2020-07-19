using TimeTrack.Application.Common.Configuration;
using TimeTrack.Core;

namespace TimeTrack.Console.Configuration
{
    public class Configuration
    {
        public AliasDefinition[] Aliases { get; set; }
        public DependencyInjectionServiceSelectionDefinition[] DependencyInjectionServiceSelection { get; set; }
        public TimeTrackManagerConfiguration TimeTrackManagerConfiguration { get; set; }
    }
}