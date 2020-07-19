using System;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TimeTrack.Application.Common
{
    public class PluginLoader
    {
        private readonly string _applicationRootPath;

        public PluginLoader(string applicationRootPath)
        {
            _applicationRootPath = applicationRootPath;
        }

        public string PluginRootFolder { get; set; } = "plugins";

        public static PluginLoader CreateFromEntryAssembly()
        {
            var appPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            return new PluginLoader(appPath);
        }

        public static Assembly ResolvePluginDependencyAssembly(object sender, ResolveEventArgs args)
        {
            if (args.RequestingAssembly != null)
            {
                var sourceLocation = Path.GetDirectoryName(args.RequestingAssembly.Location);
                var assemblyName = new AssemblyName(args.Name);

                var potentialAssembly = Path.Combine(sourceLocation, $"{assemblyName.Name}.dll");
                if (File.Exists(potentialAssembly))
                {
                    return Assembly.LoadFile(potentialAssembly);
                }
            }
            return null;
        }

        public T LoadPlugins<T>(params string[] pluginFilePatterns) where T : class, new()
        {
            var container = CreateCompositionHost(Path.Combine(_applicationRootPath, PluginRootFolder), pluginFilePatterns);

            var plugins = new T();
            container.SatisfyImports(plugins);

            return plugins;
        }

        private static CompositionHost CreateCompositionHost(string directory, string[] pluginFilePatterns)
        {
            // Find and load all the DLLs in the folder
            var assemblies = pluginFilePatterns.SelectMany(x => Directory.GetFiles(directory, x, SearchOption.AllDirectories))
              .Select(path => Assembly.LoadFile(path))
              .Where(x => x != null);

            // Add the loaded assemblies to the container
            var configuration = new ContainerConfiguration()
              .WithAssemblies(assemblies);
            //.WithAssembly(Assembly.GetEntryAssembly());

            return configuration.CreateContainer();
        }
    }
}