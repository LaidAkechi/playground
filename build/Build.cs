using Nuke.Common;
using Nuke.Common.Execution;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
partial class Build : NukeBuild
{
    public static int Main () => Execute<Build>();

    Target Hello => _ => _
        .Executes(() =>
        {
        });

    Target World => _ => _
        .DependsOn(Hello)
        .Executes(() =>
        {
        });
}
