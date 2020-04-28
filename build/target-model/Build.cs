using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using static Nuke.Common.Tools.Git.GitTasks;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    // Invoke
    //  $ nuke --plan
    //  $ nuke
    //  $ nuke integration-tests
    //  $ nuke publish
    //  $ nuke publish --skip tests
    public static int Main() => Execute<Build>(x => x.Compile);

    [GitRepository] readonly GitRepository Repository;
    [Parameter] readonly string ApiKey;

    Target Clean => _ => _
        .Before(Restore);

    Target Restore => _ => _;

    Target Compile => _ => _
        .DependsOn(Restore);

    Target Pack => _ => _
        .DependsOn(Compile)
        .After(UpdateChangelog);

    Target UnitTests => _ => _
        .DependsOn(Compile)
        .DependentFor(Tests);

    Target IntegrationTests => _ => _
        .DependsOn(Compile)
        .DependentFor(Tests);

    Target Tests => _ => _
        .DependsOn(Compile)
        .WhenSkipped(DependencyBehavior.Skip);

    Target UpdateChangelog => _ => _
        .OnlyWhenStatic(() => Repository.IsOnReleaseBranch() ||
                              Repository.IsOnHotfixBranch());

    Target Publish => _ => _
        .Requires(() => ApiKey)
        .Requires(() => GitHasCleanWorkingCopy())
        .DependsOn(Clean, Tests, UpdateChangelog, Pack);

    Target Announce => _ => _
        .TriggeredBy(Publish);
}
