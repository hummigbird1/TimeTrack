using System.Collections.Generic;
using TimeTrack.Console.Options;

namespace TimeTrack.Console.Interfaces
{
    public interface ICommandlineArgumentParser
    {
        bool TryParseArguments(IEnumerable<string> arguments, out ICommandOption commandOption);

        void WriteHelpTextToOutput(IEnumerable<string> arguments);
    }
}