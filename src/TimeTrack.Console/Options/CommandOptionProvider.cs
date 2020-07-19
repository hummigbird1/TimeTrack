using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeTrack.Console.Options
{
    public class CommandOptionProvider : ICommandOptionProvider
    {
        public Type[] OptionTypes { get; } = FindOptionTypes().ToArray();

        private static IEnumerable<Type> FindOptionTypes()
        {
            return typeof(CommonOptionsBase).Assembly.GetTypes()
                .Where(x => x.BaseType == typeof(CommonOptionsBase) && !x.IsAbstract);
        }
    }
}