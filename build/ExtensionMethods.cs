using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.GitVersion;
using System;
using System.Collections.Generic;
using System.Text;

public static class ExtensionMethods
{
    public static IEnumerable<AbsolutePath> GetOutputPathsOfProject(this Project project, Configuration configuration = null)
    {
        foreach (var rop in project.GetRelativeOutputPathsOfProject(configuration))
        {
            yield return project.Directory / rop;
        }
    }

    public static IEnumerable<RelativePath> GetRelativeOutputPathsOfProject(this Project project, Configuration configuration = null)
    {
        var outputPath = (RelativePath)project.GetMSBuildProject(configuration).GetProperty("OutputPath")?.EvaluatedValue;
        if (string.IsNullOrWhiteSpace(outputPath))
        {
            throw new InvalidOperationException($"Output path of project '{project.Name}' was empty!");
        }

        var appendTargetFramework = true;
        var boolString = project.GetProperty<string>("AppendTargetFrameworkToOutputPath");
        if (!string.IsNullOrWhiteSpace(boolString))
        {
            if (!bool.TryParse(boolString, out appendTargetFramework))
            {
                throw new InvalidOperationException($"Unexpected value of AppendTargetFrameworkToOutputPath in project '{project.Name}'!");
            }
        }
        var targetFrameworks = project.GetTargetFrameworks();
        if (appendTargetFramework && targetFrameworks.Count > 1)
        {
            foreach (var fw in targetFrameworks)
            {
                yield return outputPath / fw;
            }
        }
        else
        {
            yield return outputPath;
        }
    }

    public static string ToReleaseVersion(this GitVersion gitVersion, Configuration configuration, string framework, string runtime)
    {
        var sb = new StringBuilder();
        sb.Append(gitVersion.SemVer);
        sb.Append("+");
        sb.Append(gitVersion.BuildMetaDataPadded);
        sb.Append(".Branch.");
        sb.Append(gitVersion.BranchName.Replace("/", "-").Replace("_", "-"));
        sb.Append("+");
        sb.Append(gitVersion.Sha.Substring(0, 8));
        sb.Append("+");
        sb.AppendFormat("{0}.{1}.{2}", framework, runtime, configuration);
        return sb.ToString();
    }
}