using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeTrack.Application.Common;
using TimeTrack.Application.Common.Configuration;
using TimeTrack.Application.Common.Interfaces;
using TimeTrack.Application.Common.TextTemplating;
using TimeTrack.Interfaces;
using TimeTrack.Status.UI.Notification.Toast;

namespace TimeTrack.Status.UI.DependencyInjection
{
    public static class ServiceProviderFactory
    {
        public static IServiceProvider BuildServiceProvider(IConfigurationRoot configuration, Plugins plugins)
        {
            var typedConfiguration = new Configuration.Configuration();
            configuration.Bind(typedConfiguration);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(x => configuration);
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddSingleton(typedConfiguration);
            serviceCollection.AddSingleton<IReadOnlyCollection<DependencyInjectionServiceSelectionDefinition>>(typedConfiguration.DependencyInjectionServiceSelection ?? Enumerable.Empty<DependencyInjectionServiceSelectionDefinition>().ToArray());

            foreach (var factoryInstance in plugins.DataStorageFactories)
            {
                serviceCollection.AddTransient(typeof(IDataStorageFactory), x => factoryInstance);
            }

            serviceCollection.AddTransient<ITextTemplateRenderer, MorestachioTextTemplateRenderer>();
            serviceCollection.AddTransient<ITimeServiceProvider, SystemTimeServiceProvider>();
            serviceCollection.AddTransient<IReminder, ToastNotificationReminder>();
            serviceCollection.AddTransient(x =>
            {
                var factory = x.GetRequiredService<IDataStorageFactory>();
                var config = x.GetRequiredService<IConfiguration>();
                return factory.CreateDataStorageInstance(config);
            });

            serviceCollection.AddTransient<IServiceTypeSelector<IDataStorageFactory>, ServiceTypeSelector<IDataStorageFactory>>();
            serviceCollection.AddScoped(serviceProvider =>
            {
                var factorySelector = serviceProvider.GetRequiredService<IServiceTypeSelector<IDataStorageFactory>>();
                return factorySelector.GetRequired().CreateDataStorageInstance(configuration);
            });

            return serviceCollection.BuildServiceProvider();
        }

        private class SystemTimeServiceProvider : ITimeServiceProvider
        {
            public DateTime Now()
            {
                return DateTime.Now;
            }
        }
    }
}