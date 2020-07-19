using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Configuration;
using System;
using System.Windows.Forms;
using TimeTrack.Application.Common;
using TimeTrack.Application.Common.Configuration;
using FormsApplication = System.Windows.Forms.Application;

namespace TimeTrack.Status.UI
{
    internal static class Program
    {
        private static IConfigurationRoot ConfigureApplication(Options x)
        {
            var configPath = x.ConfigurationPath ?? CommonConfigurationFunctions.GetDefaultConfigurationPath();
            if (string.IsNullOrWhiteSpace(configPath))
            {
                throw new InvalidOperationException("No configuration file specified!");
            }

            return CommonConfigurationFunctions.CreateConfigurationFromJsonFiles(configPath, out _);
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += PluginLoader.ResolvePluginDependencyAssembly;
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            FormsApplication.EnableVisualStyles();
            FormsApplication.SetCompatibleTextRenderingDefault(false);

            try
            {
                var result = Parser.Default.ParseArguments<Options>(args);

                if (result.Tag == ParserResultType.NotParsed)
                {
                    MessageBox.Show(HelpText.AutoBuild(result).ToString(), "Parameter error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var parsedOptions = (result as Parsed<Options>).Value;
                var configuration = ConfigureApplication(parsedOptions);

                var plugins = PluginLoader.CreateFromEntryAssembly().LoadPlugins<Plugins>("*.Application.Plugin.*.dll", "*.Status.UI.Plugin.*.dll");
                var serviceProvider = DependencyInjection.ServiceProviderFactory.BuildServiceProvider(configuration, plugins);
                var mainForm = new frmMain
                {
                    ServiceProvider = serviceProvider
                };

                FormsApplication.Run(mainForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}