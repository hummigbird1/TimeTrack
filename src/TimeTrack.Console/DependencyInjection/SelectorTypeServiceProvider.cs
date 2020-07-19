using Microsoft.Extensions.DependencyInjection;
using System;
using TimeTrack.Console.Interfaces;

namespace TimeTrack.Console.DependencyInjection
{
    internal class SelectorTypeServiceProvider<TSelector, TResult> : ISelectorTypeServiceProvider<TSelector, TResult>
    {
        protected readonly ISelectorTypeRegistrationCatalog<TSelector, TResult> _catalog;
        protected readonly IServiceProvider _serviceProvider;

        public SelectorTypeServiceProvider(IServiceProvider serviceProvider, ISelectorTypeRegistrationCatalog<TSelector, TResult> catalog)
        {
            _serviceProvider = serviceProvider;
            _catalog = catalog;
        }

        public virtual TResult this[TSelector key]
        {
            get
            {
                if (key != null && _catalog.ContainsKey(key))
                {
                    return (TResult)_serviceProvider.GetRequiredService(_catalog[key]);
                }
                if (_catalog.DefaultKey != null && _catalog.ContainsKey(_catalog.DefaultKey))
                {
                    return (TResult)_serviceProvider.GetRequiredService(_catalog[_catalog.DefaultKey]);
                }

                throw new NotImplementedException($"Selector '{key}' is not registered and default key ({_catalog.DefaultKey}) is not set or also not registered!");
            }
        }
    }
}