using Nuke.Common;
using Nuke.Common.Execution;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild, IPublishNuGet, IAnnounce
{
    public static int Main() => Execute<Build>();

    public string OutputDirectory => RootDirectory / "output" / "packages";

    Target Clean => _ => _
        .Before<IBuild>(x => x.Restore)
        .DependentFor<IPublishNuGet>(x => x.Publish)
        .Executes(() =>
        {
        });

    Target ValidatePackages => _ => _
        .DependentFor<IPublishNuGet>(x => x.Publish)
        .Executes(() =>
        {
        });

    public string Message => "New version has been published!";

    public Target Announce => _ => _
        .Inherit<IAnnounce>(x => x.Announce)
        .TriggeredBy<IPublishNuGet>(x => x.Publish);
}
