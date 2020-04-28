using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build
{
    [GitVersion] readonly GitVersion GitVersion;
    static string GlobalToolName => "global-tool";

    Target Pack => _ => _
        .Unlisted()
        .Executes(() =>
        {
            DotNetPack(_ => _
                .SetProject(RootDirectory / $"{GlobalToolName}.csproj")
                .SetVersion(GitVersion.NuGetVersionV2)
                .SetOutputDirectory(RootDirectory));
        });

    Target Install => _ => _
        .Unlisted()
        .DependsOn(Uninstall)
        .Executes(() =>
        {
            DotNet($"tool install -g {GlobalToolName} --add-source {RootDirectory} --version {GitVersion.NuGetVersionV2}");
        });

    Target Uninstall => _ => _
        .Unlisted()
        .DependsOn(Pack)
        .ProceedAfterFailure()
        .Executes(() =>
        {
            DotNet("tool uninstall -g global-tool");
        });
}
