using Nuke.Common;
using Nuke.Common.Tooling;

partial class Build
{
    [PathExecutable]
    readonly Tool Git;

    [PackageExecutable(
        packageId: "xunit.runner.console",
        packageExecutable32: "xunit.console.x86.exe",
        packageExecutable64: "xunit.console.exe",
        Framework = "net471")]
    readonly Tool Xunit;

    [LocalExecutable("./build.cmd")]
    readonly Tool BuildCmd;

    Target ExecutableViaAttributes => _ => _
        .Executes(() =>
        {
            Git(arguments: "status");
        });
}
