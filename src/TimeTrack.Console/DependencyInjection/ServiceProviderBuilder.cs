using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeTrack.Application.Common;
using TimeTrack.Application.Common.Configuration;
using TimeTrack.Application.Common.Interfaces;
using TimeTrack.Console.CommandHandlers;
using TimeTrack.Console.Configuration;
using TimeTrack.Console.Export;
using TimeTrack.Console.Interfaces;
using TimeTrack.Console.ListHandling;
using TimeTrack.Console.Options;
using TimeTrack.Console.Output;
using TimeTrack.Console.OutputFormatting;
using TimeTrack.Console.QueryHandling;
using TimeTrack.Core;
using TimeTrack.Interfaces;

namespace TimeTrack.Console.DependencyInjection
{
    internal class ServiceProviderBuilder
    {
        public static IServiceProvider BuildServiceProvider(Plugins plugins)
        {
            var serviceCollection = new ServiceCollection();

            RegisterCommonTransientServices(serviceCollection);

            RegisterCommonScopedServices(serviceCollection);

            RegisterCommandHandlerServices(serviceCollection);

            RegisterQueryHandlerServices(serviceCollection);

            RegisterExportHandlerServices(serviceCollection);

            RegisterServicesFromPlugins(serviceCollection, plugins);

            return serviceCollection.BuildServiceProvider();
        }

        private static IConfigurationManager GetConfigurationManager(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IConfigurationManager>();
        }

        private static void RegisterCommandHandlers(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISelectorTypeServiceProvider<Commands, IHandler<ICommandOption, int>>, SelectorTypeServiceProvider<Commands, IHandler<ICommandOption, int>>>();

            serviceCollection.AddTransient<ExportCommandHandler>();
            serviceCollection.AddTransient<QueryCommandHandler>();
            serviceCollection.AddTransient<AliasCommandHandler>();
            serviceCollection.AddTransient<ListCommandHandler>();
            serviceCollection.AddTransient<TimeTrackManagerCommandHandler<StartOptions>>();
            serviceCollection.AddTransient<TimeTrackManagerCommandHandler<StopOptions>>();
            serviceCollection.AddTransient<CustomPluginCommandHandler>();
        }

        private static void RegisterCommandHandlerServices(ServiceCollection serviceCollection)
        {
            RegisterCommandHandlers(serviceCollection);

            var commandSelectorCatalog = new SelectorTypeRegistrationCatalog<Commands, IHandler<ICommandOption, int>>();
            commandSelectorCatalog.Add<ExportCommandHandler>(Commands.Export);
            commandSelectorCatalog.Add<QueryCommandHandler>(Commands.Query);
            commandSelectorCatalog.Add<AliasCommandHandler>(Commands.Alias);
            commandSelectorCatalog.Add<ListCommandHandler>(Commands.List);
            commandSelectorCatalog.Add<CustomPluginCommandHandler>(Commands.PluginCommand);
            commandSelectorCatalog.Add<TimeTrackManagerCommandHandler<StartOptions>>(Commands.Start);
            commandSelectorCatalog.Add<TimeTrackManagerCommandHandler<StopOptions>>(Commands.Stop);
            serviceCollection.AddSingleton<ISelectorTypeRegistrationCatalog<Commands, IHandler<ICommandOption, int>>>(commandSelectorCatalog);
        }
        private static void RegisterCommonScopedServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ILogger, OutputLogger>();
            serviceCollection.AddScoped<IConfigurationManager, ConfigurationManager>();
            serviceCollection.AddScoped(serviceProvider => GetConfigurationManager(serviceProvider).Configuration.TimeTrackManagerConfiguration);
            serviceCollection.AddScoped(serviceProvider => GetConfigurationManager(serviceProvider).ConfigurationRoot);
            serviceCollection.AddScoped(serviceProvider => GetConfigurationManager(serviceProvider).Configuration);
            serviceCollection.AddScoped<IReadOnlyCollection<DependencyInjectionServiceSelectionDefinition>>(serviceProvider => GetConfigurationManager(serviceProvider).Configuration.DependencyInjectionServiceSelection ?? Enumerable.Empty<DependencyInjectionServiceSelectionDefinition>().ToArray());

            // IDataStorage
            serviceCollection.AddScoped(serviceProvider =>
            {
                var factorySelector = serviceProvider.GetRequiredService<IServiceTypeSelector<IDataStorageFactory>>();
                var configuration = GetConfigurationManager(serviceProvider).ConfigurationRoot;
                return factorySelector.GetRequired().CreateDataStorageInstance(configuration);
            });
        }

        private static void RegisterCommonTransientServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ApplicationCore>();
            serviceCollection.AddTransient<TimeTrackManager>();

            serviceCollection.AddTransient<ICommandOptionProvider, CommandOptionProvider>();

            serviceCollection.AddTransient<IDataStorageQueryProvider, DataStorageQueryProvider>();
            serviceCollection.AddTransient<ITimeServiceProvider, SystemTimeServiceProvider>();

            serviceCollection.AddTransient<IActivityQueryProvider<TrackingActivityContainer>, CompositeActivityQueryProvider>();
            serviceCollection.AddTransient<IActivityQueryProvider<SummaryListItem>, SummaryListItemQueryProvider>();
            serviceCollection.AddTransient<IActivityQueryProvider<PlainListItem>, PlainListItemQueryProvider>();

            serviceCollection.AddTransient<IDynamicPropertySorter, DynamicPropertySorter>();

            serviceCollection.AddTransient<ICommandlineArgumentParser, CommandlineArgumentParser>();
            serviceCollection.AddTransient<IOutputWriter, ConsoleOutputWriter>();
            serviceCollection.AddTransient<IOutputBuilder, OutputBuilder>();
            serviceCollection.AddTransient<IOutputPropertyFactory, OutputPropertyFactory>();
            serviceCollection.AddTransient<IDynamicExpressionResolver, DynamicExpressionResolver>();

            serviceCollection.AddTransient<IServiceTypeSelector<IDateTimeStringParser>, ServiceTypeSelector<IDateTimeStringParser>>();
            serviceCollection.AddTransient<IServiceTypeSelector<IDataStorageFactory>, ServiceTypeSelector<IDataStorageFactory>>();
        }

        private static void RegisterExportHandlers(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISelectorTypeServiceProvider<string, IExporter>, SelectorTypeServiceProvider<string, IExporter>>();
            serviceCollection.AddTransient<PlainListExporter>();
            serviceCollection.AddTransient<SummarizedListExporter>();
        }

        private static void RegisterExportHandlerServices(ServiceCollection serviceCollection)
        {
            RegisterExportHandlers(serviceCollection);

            var exporterSelectorCatalog = new SelectorTypeRegistrationCatalog<string, IExporter>();
            exporterSelectorCatalog.Add<PlainListExporter>("plain");
            exporterSelectorCatalog.Add<SummarizedListExporter>("groupedbyidentifier");
            serviceCollection.AddSingleton<ISelectorTypeRegistrationCatalog<string, IExporter>>(exporterSelectorCatalog);
        }

        private static void RegisterQueryHandlers(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISelectorTypeServiceProvider<string, IHandler<QueryOptions, string>>, SelectorTypeServiceProvider<string, IHandler<QueryOptions, string>>>();
            serviceCollection.AddTransient<QueryIdentifierList>();
            serviceCollection.AddTransient<ReadableActiveTrackingQuery>();
            serviceCollection.AddTransient<ParsableNoActiveTrackingSinceQuery>();
            serviceCollection.AddTransient<AvailableAliasesQuery>();
            serviceCollection.AddTransient<AvailableQueryTypesQuery>();
            serviceCollection.AddTransient<AvailableExportTypesQuery>();
            serviceCollection.AddTransient<AvailableCustomPluginsQuery>();

        }

        private static void RegisterQueryHandlerServices(ServiceCollection serviceCollection)
        {
            RegisterQueryHandlers(serviceCollection);

            var querySelectorCatalog = new SelectorTypeRegistrationCatalog<string, IHandler<QueryOptions, string>>();
            querySelectorCatalog.Add<QueryIdentifierList>("lastidentifiers");
            querySelectorCatalog.Add<ReadableActiveTrackingQuery>(string.Empty);
            querySelectorCatalog.Add<ParsableNoActiveTrackingSinceQuery>("p_idle_since");
            querySelectorCatalog.Add<AvailableAliasesQuery>("available-aliases");
            querySelectorCatalog.Add<AvailableQueryTypesQuery>("available-queries");
            querySelectorCatalog.Add<AvailableExportTypesQuery>("available-export-types");
            querySelectorCatalog.Add<AvailableCustomPluginsQuery>("available-custom-plugins");


            querySelectorCatalog.DefaultKey = string.Empty;
            serviceCollection.AddSingleton<ISelectorTypeRegistrationCatalog<string, IHandler<QueryOptions, string>>>(querySelectorCatalog);
        }

        private static void RegisterServicesFromPlugins(IServiceCollection serviceCollection, Plugins plugins)
        {
            if (plugins == null)
            {
                throw new ArgumentNullException(nameof(plugins));
            }

            RegisterTransientServices<ITimeTrackingUpdatedHandler>(serviceCollection, plugins.TimeTrackingUpdatedHandlerTypes?.SelectMany(x => x.TransientTypes));

            RegisterTransientServices<IDateTimeStringParser>(serviceCollection, plugins.DateTimeStringParserTypes?.SelectMany(x => x.TransientTypes));

            RegisterTransientServices<ICustomPlugin>(serviceCollection, plugins.CustomPluginTypes?.SelectMany(x => x.TransientTypes));

            foreach (var factoryInstance in plugins.DataStorageFactories)
            {
                serviceCollection.AddTransient(typeof(IDataStorageFactory), x => factoryInstance);
            }
        }

        private static void RegisterTransientServices<TServiceType>(IServiceCollection serviceCollection, IEnumerable<Type> types)
        {
            if (types == null)
            {
                return;
            }

            foreach (var implType in types)
            {
                serviceCollection.AddTransient(typeof(TServiceType), implType);
            }
        }
    }
}