// Copyright 2020 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System.Reflection;
using Nuke.Common;
using Nuke.Common.Tooling;
using static Nuke.Common.Tooling.ProcessTasks;

partial class Build
{
    public static int Main()
    {
        if (RootDirectory.ToString().EndsWith("ci-configuration") && Help)
        {
            StartProcess(
                    toolPath: Assembly.GetEntryAssembly().NotNull().Location,
                    arguments: "--root ./ --help",
                    workingDirectory: RootDirectory.Parent.Parent)
                .AssertZeroExitCode();
            return 0;
        }

        return Execute<Build>();
    }
}
