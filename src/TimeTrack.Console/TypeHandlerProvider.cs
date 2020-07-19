using System;
using System.Collections.Generic;
using TimeTrack.Console.Interfaces;

namespace TimeTrack.Console
{
    internal class TypeHandlerProvider<TSelector, TArgument, TOutput> : Dictionary<TSelector, IHandler<TArgument, TOutput>>, IHandlerProvider<TSelector, TArgument, TOutput>
    {
        public IHandler<TArgument, TOutput> DefaultTypeHandler { get; set; }

        public IHandler<TArgument, TOutput> GetTypeHandler(TSelector input)
        {
            if (ContainsKey(input))
            {
                return this[input];
            }

            if (DefaultTypeHandler != null)
            {
                return DefaultTypeHandler;
            }

            throw new NotImplementedException($"There is no handler for '{input}' registered!");
        }
    }
}