using System;
using System.Collections.Generic;
using System.Linq;
using TimeTrack.Application.Common.Configuration;
using TimeTrack.Application.Common.Interfaces;

namespace TimeTrack.Application.Common
{
    public class ServiceTypeSelector<T> : IServiceTypeSelector<T>
    {
        private readonly IEnumerable<T> _available;
        private readonly IReadOnlyCollection<DependencyInjectionServiceSelectionDefinition> _configuration;

        public ServiceTypeSelector(IReadOnlyCollection<DependencyInjectionServiceSelectionDefinition> configuration, IEnumerable<T> available)
        {
            _configuration = configuration;
            _available = available;
        }

        public T GetRequired()
        {
            var count = _available.Count();
            if (count > 1)
            {
                return GetSingleByDefinition();
            }
            else if (count == 0)
            {
                throw new InvalidOperationException($"No providers for required service '{typeof(T).Name}' have been found!");
            }

            return _available.Single();
        }

        private T GetSingleByDefinition()
        {
            var count = _available.Count();
            var availableTypeNames = string.Join(", ", _available.Select(x => x.GetType().Name));
            var definition = _configuration?.SingleOrDefault(x => x.ServiceType == typeof(T).Name);
            if (definition == null)
            {
                throw new InvalidOperationException($"{count} providers for service '{typeof(T).Name}' have been found and no configuration was provided on which to use! (Available Types: {availableTypeNames})");
            }

            var instance = _available.SingleOrDefault(x => string.Equals(x.GetType().Name, definition.TypeName, StringComparison.OrdinalIgnoreCase));
            if (instance != null)
            {
                return instance;
            }

            throw new InvalidOperationException($"{count} providers for service '{typeof(T).Name}' have been found and the specified configuration value '{definition.TypeName}' was not valid! (Available Types: {availableTypeNames})");
        }
    }
}