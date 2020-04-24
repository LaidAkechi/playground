using System.Collections.Generic;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI.TeamCity;
using Nuke.Common.CI.TeamCity.Configuration;
using Nuke.Common.Execution;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities.Collections;

[TeamCity(
    TeamCityAgentPlatform.Unix,
    VcsTriggeredTargets = new[] {nameof(Test)},
    NonEntryTargets = new[] {nameof(Compile)},
    NightlyTriggeredTargets = new[] {nameof(Test)},
    ManuallyTriggeredTargets = new[] {nameof(Publish)})]
partial class Build
{
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
                {nameof(Pack), "📦"},
                {nameof(Publish), "🚚"},
                {nameof(Test), "🚦"}
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
