using System.IO;
using System.Reflection;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.GitVersion;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.Logger;

[UnsetVisualStudioEnvironmentVariables]
partial class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Default);

    Target Default => _ => _
        .Executes(() =>
        {
            GitVersionTasks.GitVersion(_ => _
                .SetFramework("netcoreapp3.0"));
        });
}
