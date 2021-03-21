// Copyright 2020 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System.Reflection;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;
using static Nuke.Common.CI.BuildServerConfigurationGenerationAttributeBase;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.Tooling.ProcessTasks;

partial class Build
{
    public static int Main()
    {
        if (RootDirectory.ToString().EndsWith("ci-configuration"))
        {
            StartProcess(
                    toolPath: Assembly.GetEntryAssembly().NotNull().Location,
                    arguments: $"{CommandLineArguments.JoinSpace()} --root ./",
                    workingDirectory: RootDirectory.Parent.Parent)
                .AssertZeroExitCode();
            return 0;
        }

        return Execute<Build>();
    }
}
