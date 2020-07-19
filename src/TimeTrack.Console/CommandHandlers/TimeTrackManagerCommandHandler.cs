using System;
using System.Collections.Generic;
using System.Linq;
using TimeTrack.Console.Interfaces;
using TimeTrack.Console.Options;
using TimeTrack.Core;

namespace TimeTrack.Console.CommandHandlers
{
    internal class TimeTrackManagerCommandHandler<TOptionType> : IHandler<ICommandOption, int> where TOptionType : ICommandOption
    {
        private readonly ILogger _logger;
        private readonly ICollection<ITimeTrackingUpdatedHandler> _timeTrackingUpdatedHandlers;
        private readonly TimeTrackManager _timeTrackManager;

        public TimeTrackManagerCommandHandler(TimeTrackManager timeTrackManager,
            ILogger logger,
            IEnumerable<ITimeTrackingUpdatedHandler> timeTrackingUpdatedHandlers)
        {
            _timeTrackManager = timeTrackManager;
            _timeTrackingUpdatedHandlers = timeTrackingUpdatedHandlers.ToList();
            _logger = logger;
        }

        public int Handle(ICommandOption argument)
        {
            return ExecuteCommand((TOptionType)argument);
        }

        protected int ExecuteCommand(TOptionType argument)
        {
            try
            {
                if (argument is StartOptions startArguments)
                {
                    _timeTrackManager.StartTrackingNow(startArguments.Identifier);
                    return (int)ExitCodes.Successful;
                }

                if (argument is StopOptions stopArguments)
                {
                    if (stopArguments.Discard)
                    {
                        _timeTrackManager.Discard();
                    }
                    else
                    {
                        _timeTrackManager.StopTrackingNow();
                    }

                    return (int)ExitCodes.Successful;
                }
            }
            finally
            {
                foreach (var handler in _timeTrackingUpdatedHandlers)
                {
                    try
                    {
                        handler.OnTimeTrackingUpdated();
                    }
                    catch (Exception ex)
                    {
                        _logger.Verbose($"Handler '{handler.GetType()}' had exception: {ex}");
                    }
                }
            }

            throw new NotSupportedException($"Argument of type {argument.GetType()} is not supported!");
        }
    }
}