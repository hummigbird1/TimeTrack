using Nuke.Common.IO;

internal class ReleaseTargets
{
    private readonly string _releaseName;

    public ReleaseTargets(SolutionMetadata solution, string releaseName)
    {
        TypedSolution = solution;
        _releaseName = releaseName;
    }

    public AbsolutePath ConfigurationFolder => PackageRoot / SubfolderNames.ApplicationConfiguration;
    public AbsolutePath DataFolder => PackageRoot / SubfolderNames.ApplicationData;
    public AbsolutePath KeypirhinaPlugin => PackageRoot / SubfolderNames.OutputKeypirhinaPlugin;
    public AbsolutePath PackageRoot => TypedSolution.OutputDirectory / _releaseName;
    public string PackgeArchiveFilePath => TypedSolution.OutputDirectory / _releaseName + ".zip";

    public AbsolutePath PowershellCmdlet => PackageRoot / SubfolderNames.OutputReleasePowershellCmdlet;
    private SolutionMetadata TypedSolution { get; }

    public AbsolutePath GetTargetForPlugin(string pluginName)
    {
        return PackageRoot / SubfolderNames.ApplicationPlugins / pluginName;
    }
}