using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[UnsetVisualStudioEnvironmentVariables]
partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>();

    [Solution] readonly Solution Solution;
    [Parameter] readonly Configuration Configuration = Configuration.Debug;

    // String interpolation
    Target Simple => _ => _
        .Executes(() =>
        {
            DotNet($"restore {Solution}");
            DotNet($"build {Solution} --configuration {Configuration} --no-restore");
        });
}
