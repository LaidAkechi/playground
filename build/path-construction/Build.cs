using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.Logger;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Simple);

    Target Simple => _ => _
        .Executes(() =>
        {
            Normal(RootDirectory);

            Info("Division operator:");
            Normal(RootDirectory / ".." / ".." / "src");
            Normal(RootDirectory / ".." / ".." / "output");

            Info("Absolute paths:");
            (RootDirectory / ".." / "..").GlobDirectories("tests/*")
                .ForEach(x => Normal(x));

            Info("Windows-relative paths:");
            (RootDirectory / ".." / "..").GlobDirectories("tests/*")
                .ForEach(x => Normal(RootDirectory.GetWinRelativePathTo(x)));
        });
}
