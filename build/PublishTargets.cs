using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

internal class PublishTargets
{
    private readonly string _releaseSubfolder;
    private readonly string _releaseVersion;
    private readonly Solution _solution;

    public PublishTargets(Solution solution, string buildVersion)
    {
        _solution = solution;
        _releaseVersion = buildVersion;
        _releaseSubfolder = "Published " + _releaseVersion;
    }

    public AbsolutePath KeypirhinaPluginFile => KeypirhinaPluginFolder / Constants.KeypirhinaPluginFileName;
    public AbsolutePath KeypirhinaPluginFolder => ReleaseDirectory / SubfolderNames.OutputKeypirhinaPlugin;
    public string PackgeArchiveFilePath => TypedSolution.OutputDirectory / $"Published-{_releaseVersion}.zip";

    public AbsolutePath PublishedPlugins => ReleaseDirectory / SubfolderNames.ApplicationPlugins;
    public AbsolutePath ReleaseDirectory => TypedSolution.OutputDirectory / _releaseSubfolder;
    private SolutionMetadata TypedSolution => (SolutionMetadata)_solution;

    public AbsolutePath GetPublishTargetForProject(Project project, AbsolutePath rootOutputFolder = null)
    {
        return GetPublishTargetForProject(project.Name, rootOutputFolder);
    }

    public AbsolutePath GetPublishTargetForProject(string projectName, AbsolutePath rootOutputFolder = null)
    {
        if (rootOutputFolder == null)
        {
            return ReleaseDirectory / projectName;
        }

        return rootOutputFolder / projectName;
    }

    public AbsolutePath GetPublishTargetForProjectDebuggingSymbols(Project project, AbsolutePath rootOutputFolder = null)
    {
        return GetPublishTargetForProjectDebuggingSymbols(project.Name, rootOutputFolder);
    }

    public AbsolutePath GetPublishTargetForProjectDebuggingSymbols(string projectName, AbsolutePath rootOutputFolder = null)
    {
        var subfolderName = SolutionMetadata.GetDebuggingSymbolsFolderName(projectName);
        if (rootOutputFolder == null)
        {
            return ReleaseDirectory / subfolderName;
        }
        return rootOutputFolder / subfolderName;
    }
}