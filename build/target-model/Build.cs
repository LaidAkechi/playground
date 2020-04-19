using Nuke.Common;
using Nuke.Common.Execution;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    // Invoke: nuke --plan
    public static int Main() => Execute<Build>(x => x.Compile);

    Target Clean => _ => _
        .Before(Restore);

    Target Restore => _ => _;

    Target Compile => _ => _
        .DependsOn(Restore);

    Target Pack => _ => _
        .DependsOn(Compile);

    Target Test => _ => _
        .DependsOn(Compile);

    Target Publish => _ => _
        .DependsOn(Clean, Test, Pack);

    Target Announce => _ => _
        .TriggeredBy(Publish);
}
