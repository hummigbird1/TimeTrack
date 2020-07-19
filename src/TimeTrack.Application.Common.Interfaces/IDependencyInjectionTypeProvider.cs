using System;
using System.Collections.Generic;

namespace TimeTrack.Application.Common.Interfaces
{
    public interface IDependencyInjectionTypeProvider<T>
    {
        IEnumerable<Type> TransientTypes { get; }
    }
}