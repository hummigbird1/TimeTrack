using System;

namespace TimeTrack.Console.Options
{
    public interface ICommandOptionProvider
    {
        Type[] OptionTypes { get; }
    }
}