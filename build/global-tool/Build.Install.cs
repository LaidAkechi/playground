using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build
{
    static string GlobalToolPackageName => "global-tool";

    Target Pack => _ => _
        .Unlisted()
        .Executes(() =>
        {
            DotNetPack(_ => _
                .SetProject(RootDirectory / $"{GlobalToolPackageName}.csproj")
                .SetOutputDirectory(RootDirectory));
        });

    Target Install => _ => _
        .Unlisted()
        .DependsOn(Uninstall)
        .Executes(() =>
        {
            DotNet($"tool install -g {GlobalToolPackageName} --add-source {RootDirectory}");
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
