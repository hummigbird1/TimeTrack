using System;
using System.Collections.Generic;

namespace TimeTrack.Console.Interfaces
{
    public interface ISelectorTypeRegistrationCatalog<TSelector, T>
    {
        TSelector DefaultKey { get; }

        IReadOnlyCollection<TSelector> Keys { get; }
        IReadOnlyCollection<T> Values { get; }
        Type this[TSelector key] { get; }

        void Add<TType>(TSelector key);

        bool ContainsKey(TSelector key);
    }
}