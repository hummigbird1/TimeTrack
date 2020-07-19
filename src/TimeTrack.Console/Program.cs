using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using TimeTrack.Application.Common;
using TimeTrack.Console.DependencyInjection;
using TimeTrack.Console.Exceptions;

namespace TimeTrack.Console
{
    internal class Program
    {

        internal static int Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += PluginLoader.ResolvePluginDependencyAssembly;
            int exitCode = (int)ExitCodes.Undefined;
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

#if DEBUG && DEBUG_PLUGIN_LAUNCH
            System.Diagnostics.Debugger.Launch();
#endif

            Plugins plugins;
            try
            {
                plugins = PluginLoader.CreateFromEntryAssembly().LoadPlugins<Plugins>("*.Application.Plugin.*.dll", "*.Console.Plugin.*.dll");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return (int)ExitCodes.PluginError;
            }

            var confirmExit = false;
            try
            {
                var coreServiceProvider = ServiceProviderBuilder.BuildServiceProvider(plugins);
                using (var scopedServiceProvider = coreServiceProvider.CreateScope())
                {
                    exitCode = RunApplicationCore(args, scopedServiceProvider.ServiceProvider, out confirmExit);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                exitCode = (int)ExitCodes.UnhandledException;
            }
            finally
            {
                if (confirmExit)
                {
                    System.Console.WriteLine("Press [ENTER] to quit ...");
                    System.Console.ReadLine();
                }
            }

            return exitCode;
        }


        private static int RunApplicationCore(string[] args, IServiceProvider serviceProvider, out bool confirmExit)
        {
            confirmExit = false;
            int exitCode;
            try
            {
                var core = serviceProvider.GetRequiredService<ApplicationCore>();
                exitCode = core.ExecuteCommandByArguments(args, out confirmExit);
            }
            catch (ExceptionBase ex)
            {
                System.Console.WriteLine(ex);
                exitCode = (int)ex.ExitCode;
            }

            return exitCode;
        }
    }
}