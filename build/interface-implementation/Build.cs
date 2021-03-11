using Nuke.Common;
using Nuke.Common.Execution;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild, IPublishNuGet, IAnnounce
{
    public static int Main() => Execute<Build>();

    public string OutputDirectory => RootDirectory / "output" / "packages";

    Target Clean => _ => _
        .Before<IBuild>()
        .DependentFor<IPublishNuGet>()
        .Executes(() =>
        {
        });

    Target ValidatePackages => _ => _
        .DependentFor<IPublishNuGet>()
        .Executes(() =>
        {
        });

    public string Message => "New version has been published!";

    public Target Announce => _ => _
        .Inherit<IAnnounce>()
        .TriggeredBy<IPublishNuGet>();
}
