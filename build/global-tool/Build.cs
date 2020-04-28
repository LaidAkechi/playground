using System.IO;
using Nuke.Common;
using Nuke.Common.Execution;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.Logger;

[UnsetVisualStudioEnvironmentVariables]
partial class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Default);

    Target Default => _ => _
        .Executes(() =>
        {
            Info($"Current working directory: {WorkingDirectory}");
            Info($"Number of directories: {Directory.GetDirectories(WorkingDirectory).Length}");
            Info($"Number of files: {Directory.GetFiles(WorkingDirectory).Length}");
        });
}
