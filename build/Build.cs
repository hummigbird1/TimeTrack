using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[MSBuildVerbosityMapping]
[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
internal class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    private readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Publish Framework target")]
    private readonly string Framework = "netcoreapp3.1";

    [GitRepository] private readonly GitRepository GitRepository;

    [GitVersion] private readonly GitVersion GitVersion;

    [Parameter("Skips cleaning redundant plugin assembly files")]
    private readonly bool LeaveRedundantPluginFiles;

    [Parameter("The plugin name for the DistributePlugin method")]
    private readonly string PluginName;

    [Parameter("Publish Runtime target")]
    private readonly string Runtime = "win-x64";

    [Solution] private readonly Solution Solution;

    private Target BuildRelease => _ => _
     .DependsOn(PublishAll)
     .Executes(() =>
     {
         var releaseName = string.Format(Constants.ReleaseNamePattern, BuildVersion);
         var releaseTargets = new ReleaseTargets(TypedSolution, releaseName);
         var packageTarget = releaseTargets.PackageRoot;

         var consoleSource = PublishTargets.GetPublishTargetForProject(TypedSolution.ConsoleProject);
         var statusUiSource = PublishTargets.GetPublishTargetForProject(TypedSolution.StatusUIProject);
         EnsureCleanDirectory(packageTarget);
         DeleteFile(releaseTargets.PackgeArchiveFilePath);

         // Copy KP Plugins
         CopyFileToDirectory(PublishTargets.KeypirhinaPluginFile, releaseTargets.KeypirhinaPlugin);

         // Copy Console and Status UI
         CopyDirectoryRecursively(consoleSource, packageTarget, DirectoryExistsPolicy.Merge);
         CopyDirectoryRecursively(statusUiSource, packageTarget, DirectoryExistsPolicy.Merge, FileExistsPolicy.Skip);

         // Copy Plugins
         var pluginsInComposition = new[] {
            KnownProjectName.LiteDbStoragePlugin,
            KnownProjectName.DateTimeParserPlugin };
         foreach (var plugin in pluginsInComposition)
         {
             var pluginSource = PublishTargets.GetPublishTargetForProject(plugin, PublishTargets.PublishedPlugins);
             var pluginTarget = releaseTargets.GetTargetForPlugin(plugin);
             CopyDirectoryRecursively(pluginSource, pluginTarget);
             if (!LeaveRedundantPluginFiles)
             {
                 CommonFunctions.RemoveBaseApplicationAssembliesFromPluginFolder(packageTarget, pluginTarget);
             }
         }

         // Copy Powershell Cmdlet package
         CopyDirectoryRecursively(PublishTargets.GetPublishTargetForProject(TypedSolution.PowershellCmdletProject), releaseTargets.PowershellCmdlet);

         // Default Configuration
         CopyDefaultConfigurationFiles(releaseTargets);
         EnsureExistingDirectory(releaseTargets.DataFolder);
         CopyFileToDirectory(TypedSolution.ChangelogFile, releaseTargets.PackageRoot);

         // Create Zip
         Logger.Info($"Zipping release files into {releaseTargets.PackgeArchiveFilePath} ... ");
         ZipFile.CreateFromDirectory(packageTarget, releaseTargets.PackgeArchiveFilePath, CompressionLevel.Optimal, false);
     });

    private string BuildVersion => GitVersion.ToReleaseVersion(Configuration, Framework, Runtime);

    private Target Clean => _ => _
                 .Before(Restore)
         .Executes(() =>
         {
             TypedSolution.SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
             EnsureCleanDirectory(TypedSolution.OutputDirectory);
         });

    private Target Compile => _ => _
         .DependsOn(Restore)
         .Executes(() =>
         {
             Environment.SetEnvironmentVariable(Constants.BlockDistributePluginCallEnvironmentVariable, "true");

             DotNetBuild(s => s
                  .SetProjectFile(Solution)
                  .SetConfiguration(Configuration)
                  .SetAssemblyVersion(GitVersion.AssemblySemVer)
                  .SetFileVersion(GitVersion.AssemblySemFileVer)
                  .SetInformationalVersion(GitVersion.InformationalVersion)
                  .EnableNoRestore());
         });

    private Target CompileAndDistributeAllPlugins => _ => _
     .DependsOn(Compile, DistributeAllPlugins);

    private Target DistributeAllPlugins => _ => _
         .After(Compile)
         .Executes(() =>
         {
             var targetDirectories = CommonFunctions.GetPluginTargetDirectories(Solution, Configuration);

             foreach (var pluginProject in TypedSolution.PluginProjects)
             {
                 var exceptions = new List<Exception>();
                 try
                 {
                     CommonFunctions.DistributePluginOutputToTargetDirectories(pluginProject, targetDirectories, Configuration, LeaveRedundantPluginFiles);
                 }
                 catch (Exception ex)
                 {
                     exceptions.Add(ex);
                 }

                 if (exceptions.Any())
                     throw new AggregateException(exceptions);
             }
         });

    private Target DistributePlugin => _ => _
             .After(Compile)
         .Executes(() =>
         {
             if (string.IsNullOrWhiteSpace(PluginName))
             {
                 throw new InvalidOperationException($"Parameter {nameof(PluginName)} was not specified or empty value!");
             }

             var env = Environment.GetEnvironmentVariable(Constants.BlockDistributePluginCallEnvironmentVariable);
             if (!string.IsNullOrWhiteSpace(env))
             {
                 Logger.Info($"Detected nuke build ... Distribute plugin skipped ...");
                 return;
             }

             var pluginProject = TypedSolution.PluginProjects.Single(x => x.Name == PluginName);
             var targetDirectories = CommonFunctions.GetPluginTargetDirectories(Solution, Configuration);
             CommonFunctions.DistributePluginOutputToTargetDirectories(pluginProject, targetDirectories, Configuration, LeaveRedundantPluginFiles);
         });

    private Target PublishAll => _ => _
         .DependsOn(PublishConsoleApp, PublishStatusUI, PublishAllPlugins, PublishPowershellCmdlet, PublishKeypirhinaPlugin)
    .Executes(() =>
    {
        DeleteFile(PublishTargets.PackgeArchiveFilePath);
        Logger.Info($"Zipping published files into {PublishTargets.PackgeArchiveFilePath} ... ");
        ZipFile.CreateFromDirectory(PublishTargets.ReleaseDirectory, PublishTargets.PackgeArchiveFilePath, CompressionLevel.Optimal, false);
    });

    private Target PublishAllPlugins => _ => _
     .Executes(() =>
     {
         Environment.SetEnvironmentVariable(Constants.BlockDistributePluginCallEnvironmentVariable, "true");

         EnsureCleanDirectory(PublishTargets.PublishedPlugins);
         foreach (var pluginProject in TypedSolution.PluginProjects)
         {
             DotnetPublishProjectToTempLocation(pluginProject, PublishTargets.PublishedPlugins, Configuration, Runtime, null);
         }
     });

    private Target PublishConsoleApp => _ => _
         .Executes(() =>
     {
         DotnetPublishProjectToTempLocation(TypedSolution.ConsoleProject, PublishTargets.ReleaseDirectory, Configuration, Runtime, Framework);
     });

    private Target PublishKeypirhinaPlugin => _ => _
     .Executes(() =>
     {
         EnsureCleanDirectory(PublishTargets.KeypirhinaPluginFolder);
         ZipFile.CreateFromDirectory(TypedSolution.KeypirhinaPluginSourceFolder, PublishTargets.KeypirhinaPluginFile, CompressionLevel.Optimal, false);
     });

    private Target PublishPowershellCmdlet => _ => _
     .Executes(() =>
     {
         DotnetPublishProjectToTempLocation(TypedSolution.PowershellCmdletProject, PublishTargets.ReleaseDirectory, Configuration, Runtime, Framework);
     });

    private Target PublishStatusUI => _ => _
         .Executes(() =>
         {
             DotnetPublishProjectToTempLocation(TypedSolution.StatusUIProject, PublishTargets.ReleaseDirectory, Configuration, Runtime, Framework);
         });

    private PublishTargets PublishTargets => new PublishTargets(Solution, BuildVersion);

    private Target Restore => _ => _
             .Executes(() =>
         {
             DotNetRestore(s => s
                 .SetProjectFile(Solution)
                 );
         });

    private SolutionMetadata TypedSolution => (SolutionMetadata)Solution;

    public static int Main() => Execute<Build>(x => x.Compile);

    private void CopyDefaultConfigurationFiles(ReleaseTargets releaseTargets)
    {
        var solutionFolder = Solution.AllSolutionFolders.Single(x => x.Name == SubfolderNames.DefaultConfigurationSourceFiles);
        var items = solutionFolder.Items;

        foreach (var item in solutionFolder.Items)
        {
            var file = TypedSolution.SourceDirectory / item.Value;
            CopyFileToDirectory(file, releaseTargets.ConfigurationFolder, createDirectories: true);
        }
    }

    private void DotnetPublishProjectToTempLocation(Project project, AbsolutePath rootOutputFolder, Configuration configuration, string runtime, string framework)
    {
        var publishTarget = TemporaryDirectory / project.Name;

        EnsureCleanDirectory(publishTarget);
        DotNetPublish(s =>
        {
            s = s.SetOutput(publishTarget)
            .SetProject(project)
            .SetConfiguration(configuration)
            .SetRuntime(runtime)
            .SetAssemblyVersion(GitVersion.AssemblySemVer)
            .SetFileVersion(GitVersion.AssemblySemFileVer)
            .SetInformationalVersion(GitVersion.InformationalVersion);
            if (project.GetTargetFrameworks().Count > 1)
            {
                s = s.SetFramework(framework);
            }
            return s;
        });

        var assemblyOutput = PublishTargets.GetPublishTargetForProject(project, rootOutputFolder);
        var debugSymbolsOutput = PublishTargets.GetPublishTargetForProjectDebuggingSymbols(project, rootOutputFolder);
        CommonFunctions.SplitCopyOutputAndDebuggingSymbols(publishTarget, assemblyOutput, debugSymbolsOutput);
    }
}