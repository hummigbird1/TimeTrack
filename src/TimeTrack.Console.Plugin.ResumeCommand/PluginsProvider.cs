using System;
using System.Collections.Generic;
using System.Composition;
using TimeTrack.Application.Common.Interfaces;
using TimeTrack.Console.Interfaces;

namespace TimeTrack.Console.Plugin.ResumeCommand
{
    [Export(typeof(IDependencyInjectionTypeProvider<ICustomPlugin>))]
    public class AssemblyPluginsProvider : IDependencyInjectionTypeProvider<ICustomPlugin>
    {
        public IEnumerable<Type> TransientTypes => new[] { typeof(ResumeCommand) };
    }
}
