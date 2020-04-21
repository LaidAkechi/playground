using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Install);

    [GitVersion] readonly GitVersion GitVersion;
    static string GlobalToolName => "global-tool";

    Target Pack => _ => _
        .Executes(() =>
        {
            DotNetPack(_ => _
                .SetProject(RootDirectory / $"{GlobalToolName}.csproj")
                .SetVersion(GitVersion.NuGetVersionV2)
                .SetOutputDirectory(RootDirectory));
        });

    Target Install => _ => _
        .DependsOn(Uninstall)
        .Executes(() =>
        {
            DotNet($"tool install -g {GlobalToolName} --add-source {RootDirectory} --version {GitVersion.NuGetVersionV2}");
        });

    Target Uninstall => _ => _
        .DependsOn(Pack)
        .ProceedAfterFailure()
        .Executes(() =>
        {
            DotNet($"tool uninstall -g global-tool");
        });
}
