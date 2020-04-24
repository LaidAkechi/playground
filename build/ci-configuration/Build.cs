using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build : NukeBuild
{
    [Solution] readonly Solution Solution;
    [Parameter] readonly Configuration Configuration = Configuration.Debug;
    string OutputDirectory => RootDirectory / "output";

    Target Compile => _ => _
        .Executes(() =>
        {
        });

    Target Pack => _ => _
        .Executes(() =>
        {
            DotNetPack(_ => _
                .SetProject(Solution.GetProject("Library"))
                .SetOutputDirectory(OutputDirectory));
        });

    Target Publish => _ => _
        .DependsOn(Pack)
        .Consumes(Pack)
        .Executes(() =>
        {
            var packages =
                DotNetNuGetPush(_ => _);
        });

    [Partition(3)] readonly Partition TestPartition;
    IEnumerable<Project> TestProjects => TestPartition.GetCurrent(Solution.GetProjects("*.Tests"));

    Target Test => _ => _
        .Partition(() => TestPartition)
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .CombineWith(TestProjects, (_, v) => _
                    .SetProjectFile(v)
                    .SetLogger($"trx;LogFileName={v.Name}.trx")));
        });
}
