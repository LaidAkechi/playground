using Nuke.Common;
using Nuke.Common.Execution;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild, IPublish, IAnnounce
{
    public static int Main() => Execute<Build>(x => ((IPublish) x).Publish);

    public string OutputDirectory => RootDirectory / "output" / "packages";

    Target Clean => _ => _
        .Before<IBuild>(x => x.Restore)
        .DependentFor<IPublish>(x => x.Publish)
        .Executes(() =>
        {
        });

    Target ValidatePackages => _ => _
        .DependentFor<IPublish>(x => x.Publish)
        .Executes(() =>
        {
        });

    public string Message => "New version has been published!";

    Target Announce => _ => _
        .Executes(((IAnnounce) this).Announce) // TODO: should not be necessary
        .TriggeredBy<IPublish>(x => x.Publish);
}
