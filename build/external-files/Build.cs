using Nuke.Common;
using Nuke.Common.Execution;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>();

    Target Foo => _ => _;
}

