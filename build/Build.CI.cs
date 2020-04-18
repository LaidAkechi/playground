using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI.AppVeyor;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.CI.TeamCity;
using Nuke.Common.CI.TeamCity.Configuration;
using Nuke.Common.Execution;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities.Collections;

// ReSharper disable InconsistentNaming

// Single node CI execution
[AppVeyor(
    AppVeyorImage.UbuntuLatest,
    AppVeyorImage.VisualStudioLatest,
    InvokedTargets = new[] {nameof(CI_Test)})]
[GitHubActions(
    "continuous",
    GitHubActionsImage.UbuntuLatest,
    GitHubActionsImage.WindowsLatest,
    InvokedTargets = new[] {nameof(CI_Test)})]

// Multi node CI execution
[AzurePipelines(
    AzurePipelinesImage.Ubuntu1604,
    AzurePipelinesImage.Ubuntu1804,
    InvokedTargets = new[] {nameof(CI_Test)},
    // Target without dedicated agent execution
    NonEntryTargets = new[] {nameof(CI_Build)})]
[TeamCity(
    TeamCityAgentPlatform.Unix,
    VcsTriggeredTargets = new[] {nameof(CI_Test)},
    NonEntryTargets = new[] {nameof(CI_Build)},
    NightlyTriggeredTargets = new[] {nameof(CI_Test)},
    ManuallyTriggeredTargets = new[] {nameof(CI_Publish)})]
partial class Build
{
    Target CI_Build => _ => _
        .Executes(() =>
        {
        });

    Target CI_Test => _ => _
        .DependsOn(CI_Build)
        .Executes(() =>
        {
        });

    Target CI_Publish => _ => _
        .DependsOn(CI_Build)
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
                {nameof(CI_Build), "⚙️"},
                {nameof(CI_Test), "🚦"},
                {nameof(CI_Publish), "🚚"}
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
