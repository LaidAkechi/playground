using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>();

    [Solution] readonly Solution Solution;

    [Parameter] readonly Configuration Configuration = Configuration.Debug;
    [Parameter] bool IgnoreFailedSources;

    // String interpolation
    Target Simple => _ => _
        .Executes(() =>
        {
            DotNet($"restore {Solution}");
            DotNet($"build {Solution} --configuration {Configuration} --no-restore");
        });

    [PathExecutable]
    readonly Tool Git;

    [PackageExecutable(
        packageId: "xunit.runner.console",
        packageExecutable32: "xunit.console.x86.exe",
        packageExecutable64: "xunit.console.exe",
        Framework = "net471")]
    readonly Tool Xunit;

    [LocalExecutable("./build.cmd")]
    readonly Tool BuildCmd;

    Target ViaAttributes => _ => _
        .Executes(() =>
        {
            Git("status");
        });

    // Fluent modifications
    Target FluentAPI => _ => _
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution)
                .SetIgnoreFailedSources(IgnoreFailedSources));

            DotNetBuild(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .SetVerbosity(DotNetVerbosity.Minimal));

            DotNetPack(_ => _
                .SetProject(Solution)
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .SetOutputDirectory(OutputDirectory));
        });

    [Parameter] bool EnableCoverage;

    // Enable code coverage via parameter and use SourceLink on server builds
    Target ConditionalApi => _ => _
        .Executes(() =>
        {
            DotNetTest(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .When(EnableCoverage, _ => _
                    .EnableCollectCoverage()
                    .SetCoverletOutputFormat(CoverletOutputFormat.cobertura)
                    .When(IsServerBuild, _ => _
                        .EnableUseSourceLink())));
        });

    // Publish multiple projects with their respective target frameworks
    Target CombinatorialApi01 => _ => _
        .Executes(() =>
        {
            var publishConfigurations =
                from project in Solution.GetProjects("*rary")
                from framework in project.GetTargetFrameworks()
                select new { project, framework };

            DotNetPublish(_ => _
                    .SetConfiguration(Configuration)
                    .CombineWith(publishConfigurations, (_, v) => _
                        .SetProject(v.project)
                        .SetFramework(v.framework)));
        });

    // Push multiple packages in parallel and aggregate errors
    AbsolutePath OutputDirectory => RootDirectory / ".." / ".." / "output";

    Target CombinatorialApi02 => _ => _
        .Executes(() =>
        {
            var packages = OutputDirectory.GlobFiles("*.nupkg").NotEmpty();

            DotNetNuGetPush(_ => _
                    .SetSource("url")
                    .SetApiKey("api-key")
                    .CombineWith(packages, (_, v) => _
                        .SetTargetPath(v)),
                degreeOfParallelism: 5,
                completeOnFailure: true);
        });
}
