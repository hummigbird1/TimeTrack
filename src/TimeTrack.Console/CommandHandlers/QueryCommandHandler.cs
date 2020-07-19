using TimeTrack.Console.Interfaces;
using TimeTrack.Console.Options;

namespace TimeTrack.Console.CommandHandlers
{
    internal class QueryCommandHandler : IHandler<ICommandOption, int>
    {
        private readonly IOutputWriter _outputWriter;
        private readonly ISelectorTypeServiceProvider<string, IHandler<QueryOptions, string>> _selectorTypeServiceProvider;

        public QueryCommandHandler(ISelectorTypeServiceProvider<string, IHandler<QueryOptions, string>> selectorTypeServiceProvider, IOutputWriter outputWriter)
        {
            _selectorTypeServiceProvider = selectorTypeServiceProvider;
            _outputWriter = outputWriter;
        }

        public int Handle(ICommandOption argument)
        {
            return ExecuteCommand((QueryOptions)argument);
        }

        protected int ExecuteCommand(QueryOptions argument)
        {
            var queryHandler = _selectorTypeServiceProvider[argument.RequestType?.ToLower()];
            var output = queryHandler.Handle(argument);
            if (output != null)
            {
                _outputWriter.WriteLine(output);
            }

            return (int)ExitCodes.Successful;
        }
    }
}