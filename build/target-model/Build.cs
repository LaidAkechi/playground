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
        .Before(Restore)
        .Executes(() =>
        {
        });

    Target Restore => _ => _
        .Executes(() =>
        {
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .After(UpdateChangelog)
        .Executes(() =>
        {
        });

    Target UnitTests => _ => _
        .DependsOn(Compile)
        .DependentFor(Tests)
        .Executes(() =>
        {
        });

    Target IntegrationTests => _ => _
        .DependsOn(Compile)
        .DependentFor(Tests)
        .Executes(() =>
        {
        });

    Target Tests => _ => _
        .DependsOn(Compile)
        .WhenSkipped(DependencyBehavior.Skip);

    Target UpdateChangelog => _ => _
        .OnlyWhenStatic(() => Repository.IsOnReleaseBranch() ||
                              Repository.IsOnHotfixBranch())
        .Executes(() =>
        {
        });

    Target Publish => _ => _
        .Requires(() => ApiKey)
        .Requires(() => GitHasCleanWorkingCopy())
        .DependsOn(Clean, Tests, UpdateChangelog, Pack)
        .Executes(() =>
        {
        });

    Target Announce => _ => _
        .TriggeredBy(Publish)
        .Executes(() =>
        {
        });
}
