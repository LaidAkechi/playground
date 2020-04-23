using System;
using System.Linq;
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
            Info(RootDirectory / ".." / ".." / "src");
            Info(RootDirectory / ".." / ".." / "output");

            (RootDirectory / ".." / "..").GlobDirectories("tests/*")
                .ForEach(x => Info(x));
        });
}
