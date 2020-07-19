using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

internal static class CommonFunctions
{
    public static void DistributePluginOutputToTargetDirectories(Project pluginProject, IList<AbsolutePath> targetDirectories, Configuration configuration, bool LeaveRedundantPluginFiles)
    {
        if (pluginProject == null)
        {
            throw new ArgumentNullException(nameof(pluginProject), "Plugin was not specified!");
        }

        var exceptions = new List<Exception>();

        var source = pluginProject.GetOutputPathsOfProject(configuration).SingleOrDefaultOrError($"Could not determine the output directory of the plugin project {pluginProject.Name} ({configuration})!");
        if (!Directory.Exists(source) || !Directory.GetFiles(source).Any())
        {
            Logger.Warn($"Plugin source directory '{source}' does not exist! Did build fail?");
            return;
        }

        foreach (var targetDirectory in targetDirectories)
        {
            try
            {
                var pluginTarget = targetDirectory / pluginProject.Name;
                FileSystemTasks.EnsureCleanDirectory(pluginTarget);
                FileSystemTasks.CopyDirectoryRecursively(source, pluginTarget, DirectoryExistsPolicy.Merge, FileExistsPolicy.Overwrite);
                AbsolutePath appBase = targetDirectory / "..";
                if (!LeaveRedundantPluginFiles)
                {
                    CommonFunctions.RemoveBaseApplicationAssembliesFromPluginFolder(appBase, pluginTarget);
                }
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }

        if (exceptions.Any())
            throw new AggregateException(exceptions);
    }

    public static List<AbsolutePath> GetPluginTargetDirectories(Solution solution, Configuration configuration)
    {
        var pluginCapableApplications = new[] {
                KnownProjectName.ConsoleApplication,
                KnownProjectName.StatusUI
             };

        var targetApplications = pluginCapableApplications.SelectMany(pName => solution.AllProjects.Where(x => x.Name == pName));
        var targetDirectories = new List<AbsolutePath>();
        foreach (var target in targetApplications.SelectMany(x => x.GetOutputPathsOfProject(configuration)))
        {
            if (Directory.Exists(target))
            {
                targetDirectories.Add(target / SubfolderNames.ApplicationPlugins);
            }
        }

        return targetDirectories;
    }

    public static bool IsFileDebugSymbol(FileInfo fileInfo)
    {
        return fileInfo.Extension == ".pdb";
    }

    public static void RemoveBaseApplicationAssembliesFromPluginFolder(string applicationPath, string pluginPath)
    {
        var appDirectory = new DirectoryInfo(applicationPath);
        var appFiles = appDirectory.GetFiles();
        var pluginDirectory = new DirectoryInfo(pluginPath);
        var searchPatterns = new[]
        {
            "*.dll",
        };

        var pluginFiles = searchPatterns.SelectMany(p => pluginDirectory.GetFiles(p));
        var fileCount = 0;
        foreach (var file in pluginFiles)
        {
            var appFile = appFiles.SingleOrDefault(af => af.Name == file.Name);
            if (appFile != null)
            {
                Logger.Trace($"Removing redundant file {file.Name} from {pluginPath}");
                file.Delete();
                var pdbFileName = Path.GetFileNameWithoutExtension(file.Name) + ".pdb";
                var pdbFile = new FileInfo(Path.Combine(pluginDirectory.FullName, pdbFileName));
                if (pdbFile.Exists)
                {
                    pdbFile.Delete();
                }
                fileCount++;
            }
        }

        if (fileCount > 0)
        {
            Logger.Info($"Removed {fileCount} redundant files from {pluginDirectory}");
        }
    }

    public static void SplitCopyOutputAndDebuggingSymbols(string source, string target, string targetDebugSymbols)
    {
        FileSystemTasks.EnsureCleanDirectory(target);
        FileSystemTasks.EnsureCleanDirectory(targetDebugSymbols);

        FileSystemTasks.CopyDirectoryRecursively(source, target, DirectoryExistsPolicy.Merge, FileExistsPolicy.Fail, excludeFile: IsFileDebugSymbol);
        FileSystemTasks.CopyDirectoryRecursively(source, targetDebugSymbols, DirectoryExistsPolicy.Merge, FileExistsPolicy.Fail, excludeFile: fi => !IsFileDebugSymbol(fi));
    }
}