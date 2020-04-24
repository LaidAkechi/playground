// Copyright 2020 Maintainers of NUKE.
// Distributed under the MIT License.
// https://github.com/nuke-build/nuke/blob/master/LICENSE

using System.Reflection;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;
using static Nuke.Common.CI.HandleConfigurationGenerationAttribute;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.Tooling.ProcessTasks;

partial class Build
{
    public static int Main()
    {
        if (RootDirectory.ToString().EndsWith("ci-configuration"))
        {
            if (Help)
            {
                StartProcess(
                        toolPath: Assembly.GetEntryAssembly().NotNull().Location,
                        arguments: "--root ./ --help",
                        workingDirectory: RootDirectory.Parent.Parent)
                    .AssertZeroExitCode();
                return 0;
            }

            if (GetParameter<string>(ConfigurationParameterName) != null &&
                GetParameter<HostType?>(() => Host) != null)
            {
                StartProcess(
                        toolPath: Assembly.GetEntryAssembly().NotNull().Location,
                        arguments: $"{CommandLineArguments.JoinSpace()} --root ./",
                        workingDirectory: RootDirectory.Parent.Parent)
                    .AssertZeroExitCode();
                return 0;
            }

            if (IsLocalBuild)
            {
                Logger.Error("This build cannot be executed locally.");
                return 1;
            }
        }

        return Execute<Build>();
    }
}
