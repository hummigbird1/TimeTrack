using System;
using System.Collections.Generic;
using System.Linq;
using TimeTrack.Console.Interfaces;
using TimeTrack.Console.Options;

namespace TimeTrack.Console.CommandHandlers
{
    internal class CustomPluginCommandHandler : IHandler<ICommandOption, int>
    {
        private readonly IEnumerable<ICustomPlugin> _pluginFactories;

        public CustomPluginCommandHandler(IEnumerable<ICustomPlugin> plugins)
        {
            _pluginFactories = plugins;
        }

        public int Handle(ICommandOption argument)
        {
            if (argument is PluginCommandOptions pluginCommandOptions)
            {
                var plugin = _pluginFactories?.SingleOrDefault(x => string.Equals(x.Name, pluginCommandOptions.Name));
                if (plugin == null)
                {
                    throw new ArgumentException($"No plugin with the name '{pluginCommandOptions.Name}' was found!");
                }

                try
                {
                    plugin.Execute(pluginCommandOptions.Command);
                    return (int)ExitCodes.Successful;
                }
                catch
                {
                    return (int)ExitCodes.UnhandledException;
                }
            }

            throw new NotSupportedException($"Argument of type {argument.GetType()} is not supported!");
        }
    }
}