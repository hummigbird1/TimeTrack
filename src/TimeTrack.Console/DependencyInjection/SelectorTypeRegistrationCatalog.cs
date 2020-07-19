using System;
using System.Collections.Generic;
using System.Linq;
using TimeTrack.Console.Interfaces;

namespace TimeTrack.Console.DependencyInjection
{
    public class SelectorTypeRegistrationCatalog<TSelector, TResultType> : Dictionary<TSelector, Type>, ISelectorTypeRegistrationCatalog<TSelector, TResultType>
    {
        public TSelector DefaultKey { get; set; }

        IReadOnlyCollection<TSelector> ISelectorTypeRegistrationCatalog<TSelector, TResultType>.Keys => Keys;

        IReadOnlyCollection<TResultType> ISelectorTypeRegistrationCatalog<TSelector, TResultType>.Values => Values.Cast<TResultType>().ToArray();

        public virtual void Add<T>(TSelector key)
        {
            // TODO Check Type implements the TResultType?
            Add(key, typeof(T));
        }
    }
}