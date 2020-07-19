using System;
using System.Collections.Generic;
using TimeTrack.Application.Common.Configuration;
using TimeTrack.Console.Configuration;
using TimeTrack.Console.Exceptions;
using TimeTrack.Console.Interfaces;
using TimeTrack.Console.Options;

namespace TimeTrack.Console
{
    internal class ApplicationCore
    {
        private readonly ICommandlineArgumentParser _commandlineParser;
        private readonly ISelectorTypeServiceProvider<Commands, IHandler<ICommandOption, int>> _commandTypesServiceProvider;
        private readonly IConfigurationManager _configurationManager;
        private readonly ILogger _logger;

        public ApplicationCore(IConfigurationManager configurationManager,
            ILogger logger,
            ISelectorTypeServiceProvider<Commands, IHandler<ICommandOption, int>> commandTypesServiceProvider,
            ICommandlineArgumentParser commandlineParser
            )
        {
            _configurationManager = configurationManager;
            _logger = logger;
            _commandTypesServiceProvider = commandTypesServiceProvider;
            _commandlineParser = commandlineParser;
        }

        public int ExecuteCommand(ICommandOption commandOptions)
        {
            SetCommonOptions(commandOptions);

            if (_configurationManager.Configuration == null)
            {
                throw new NoConfigurationSpecifiedException();
            }

            var activeConfig = _configurationManager.ConfigurationPath;
            _logger.Verbose($"Executing command {commandOptions.Command} Options: {commandOptions}  (Configuration file: {activeConfig})");

            var commandHandler = _commandTypesServiceProvider[commandOptions.Command];
            return commandHandler.Handle(commandOptions);
        }

        public int ExecuteCommandByArguments(IEnumerable<string> args, out bool confirmExit)
        {
            confirmExit = false;
            if (!_commandlineParser.TryParseArguments(args, out var commandOption))
            {
                return (int)ExitCodes.ParameterParsingError;
            }

            try
            {
                confirmExit = commandOption.ConfirmExit;
                return ExecuteCommand(commandOption);
            }
            catch (UndefinedAliasException)
            {
                _commandlineParser.WriteHelpTextToOutput(args);
                return (int)ExitCodes.ParameterParsingError;
            }
        }

        private void SetCommonOptions(ICommandOption commandOption)
        {
            _logger.Enabled = commandOption.Verbose;

            if (!string.IsNullOrWhiteSpace(commandOption.ConfigurationPath))
            {
                _logger.Verbose($"Setting configuration path: {commandOption.ConfigurationPath}");
                _configurationManager.SetConfigurationFilePath(commandOption.ConfigurationPath);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(_configurationManager.ConfigurationPath))
                {
                    // When the configuration has already been set we do not go through the default routine again
                    // We are probably within an alias call
                    return;
                }

                var configfurationPath = CommonConfigurationFunctions.GetDefaultConfigurationPath();
                if (string.IsNullOrWhiteSpace(configfurationPath))
                {
                    throw new InvalidOperationException("No configuration path was specified and default configuration could not be determined!");
                }

                _configurationManager.SetConfigurationFilePath(configfurationPath);
            }
        }
    }
}