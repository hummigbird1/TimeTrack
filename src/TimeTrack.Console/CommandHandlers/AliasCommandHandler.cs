using System;
using System.Linq;
using TimeTrack.Console.Configuration;
using TimeTrack.Console.Exceptions;
using TimeTrack.Console.Interfaces;
using TimeTrack.Console.Options;

namespace TimeTrack.Console.CommandHandlers
{
    internal class AliasCommandHandler : IHandler<ICommandOption, int>
    {
        private readonly ApplicationCore _applicationCore;
        private readonly IConfigurationManager _configurationManager;
        private readonly ILogger _logger;

        public AliasCommandHandler(IConfigurationManager configurationManager, ILogger logger, ApplicationCore applicationCore)
        {
            _configurationManager = configurationManager;
            _logger = logger;
            _applicationCore = applicationCore;
        }

        public int Handle(ICommandOption argument)
        {
            return ExecuteCommand((AliasOptions)argument);
        }

        protected int ExecuteCommand(AliasOptions argument)
        {
            var baseExitCode = (int)Commands.Alias * (int)ExitCodes.CommandSpecificBaseCode;
            var taskName = argument.Alias;

            var taskConfig = _configurationManager
                .Configuration
                .Aliases?
                .SingleOrDefault(x => string.Equals(x.Name, taskName, StringComparison.OrdinalIgnoreCase));

            if (taskConfig == null)
            {
                throw new UndefinedAliasException();
            }

            var safeArguments = (taskConfig.Arguments ?? Enumerable.Empty<string>()).ToArray();

            _logger.Verbose($"Executing alias '{taskConfig.Name}' with {safeArguments.Length} arguments ({string.Join(" ", safeArguments)})");
            var exitCode = _applicationCore.ExecuteCommandByArguments(safeArguments, out _);
            _logger.Verbose($"Alias '{taskConfig.Name}' exited with {exitCode}");
            if (exitCode != (int)ExitCodes.Successful)
            {
                return exitCode + baseExitCode;
            }

            return (int)ExitCodes.Successful;
        }
    }
}