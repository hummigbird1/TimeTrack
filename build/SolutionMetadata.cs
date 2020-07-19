using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using System.Collections.Generic;
using System.Linq;

public class SolutionMetadata
{
    public SolutionMetadata(Solution solution)
    {
        Solution = solution;
    }

    public AbsolutePath ChangelogFile => SourceDirectory / Constants.ChangelogFileName;
    public Project ConsoleProject => Solution.AllProjects.Single(x => x.Name == KnownProjectName.ConsoleApplication);

    public AbsolutePath KeypirhinaPluginRoot => SourceDirectory / SubfolderNames.KeypirhinaPluginSourceRoot;

    public AbsolutePath KeypirhinaPluginSourceFolder => KeypirhinaPluginRoot / SubfolderNames.KeypirhinaPluginContent;

    public AbsolutePath OutputDirectory => RootDirectory / SubfolderNames.OutputRoot;

    public IEnumerable<Project> PluginProjects => Solution.AllProjects.Where(x => x.SolutionFolder == PluginsFolder);

    public SolutionFolder PluginsFolder => Solution.AllSolutionFolders.Single(x => x.Name == SubfolderNames.SolutionPlugins);

    public Project PowershellCmdletProject => Solution.AllProjects.Single(x => x.Name == KnownProjectName.PowershellCmdlet);

    public AbsolutePath RootDirectory { get; } = NukeBuild.RootDirectory;

    public Solution Solution { get; }

    public AbsolutePath SourceDirectory => RootDirectory / SubfolderNames.SourcesRoot;

    public Project StatusUIProject => Solution.AllProjects.Single(x => x.Name == KnownProjectName.StatusUI);

    public static explicit operator SolutionMetadata(Solution solution)
    {
        return new SolutionMetadata(solution);
    }

    public static string GetDebuggingSymbolsFolderName(string projectName) => string.Format(Constants.DebuggingSymbolsFolderNamePattern, projectName);
}