using System.Collections.Generic;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.Logger;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Simple);

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath OutputDirectory => RootDirectory / "output";

    AbsolutePath ReportDirectory => OutputDirectory / "report";

    UnixRelativePath UnixReportDirectory
        => RootDirectory.GetUnixRelativePathTo(ReportDirectory);

    WinRelativePath WinReportDirectory
        => RootDirectory.GetWinRelativePathTo(ReportDirectory);

    IEnumerable<AbsolutePath> MarkdownFiles => RootDirectory.GlobFiles("**/*.md");

    Target Simple => _ => _
        .Executes(() =>
        {
            Info("Absolute paths:");
            Normal($"{nameof(RootDirectory)} = {RootDirectory}");
            Normal($"{nameof(SourceDirectory)} = {SourceDirectory}");
            Normal($"{nameof(OutputDirectory)} = {OutputDirectory}");
            Normal($"{nameof(ReportDirectory)} = {ReportDirectory}");

            Info("Relative paths:");
            Normal($"{nameof(UnixReportDirectory)} = {UnixReportDirectory}");
            Normal($"{nameof(WinReportDirectory)} = {WinReportDirectory}");

            Info("Globbing paths:");
            MarkdownFiles.ForEach(x => Normal(x));
        });
}
