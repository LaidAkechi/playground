using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.AppVeyor;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.CI.TeamCity;
using Nuke.Common.CI.TeamCity.Configuration;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

// ReSharper disable InconsistentNaming

// Single node CI execution
[AppVeyor(
    AppVeyorImage.UbuntuLatest,
    AppVeyorImage.VisualStudioLatest,
    InvokedTargets = new[] {nameof(Test)})]
[GitHubActions(
    "continuous",
    GitHubActionsImage.UbuntuLatest,
    GitHubActionsImage.WindowsLatest,
    InvokedTargets = new[] {nameof(Test)})]








// Multi node CI execution
[AzurePipelines(
    AzurePipelinesImage.Ubuntu1604,
    AzurePipelinesImage.Ubuntu1804,
    InvokedTargets = new[] {nameof(Test)},
    // Target without dedicated agent execution
    NonEntryTargets = new[] {nameof(Compile)})]





[TeamCity(
    TeamCityAgentPlatform.Unix,
    VcsTriggeredTargets = new[] {nameof(Test)},
    NonEntryTargets = new[] {nameof(Compile)},
    NightlyTriggeredTargets = new[] {nameof(Test)},
    ManuallyTriggeredTargets = new[] {nameof(Publish)})]



class Build : NukeBuild
{
    public static int Main() => Execute<Build>();

    [Solution] readonly Solution Solution;
    [Parameter] readonly Configuration Configuration = Configuration.Debug;

    Target Compile => _ => _
        .Executes(() =>
        {
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

    Target Publish => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
        });

    class TeamCityAttribute : Nuke.Common.CI.TeamCity.TeamCityAttribute
    {
        public TeamCityAttribute(TeamCityAgentPlatform platform)
            : base(platform)
        {
        }

        protected override IEnumerable<TeamCityBuildType> GetBuildTypes(
            NukeBuild build,
            ExecutableTarget executableTarget,
            TeamCityVcsRoot vcsRoot,
            LookupTable<ExecutableTarget, TeamCityBuildType> buildTypes)
        {
            var dictionary = new Dictionary<string, string>
            {
                {nameof(Compile), "⚙️"},
                {nameof(Test), "🚦"},
                {nameof(Publish), "🚚"}
            };

            return base.GetBuildTypes(build, executableTarget, vcsRoot, buildTypes)
                .ForEachLazy(x =>
                {
                    var symbol = dictionary.GetValueOrDefault(x.InvokedTargets.Last()).NotNull("symbol != null");
                    x.Name = x.PartitionName == null
                        ? $"{symbol} {x.Name}"
                        : $"{symbol} {x.InvokedTargets.Last()} 🧩 {x.Partition}";
                });
        }
    }
}
