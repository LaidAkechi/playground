using System;
using System.Linq;
using System.Threading;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>();
    
    [Parameter("")]
    readonly string StringParam;

    [Parameter]
    readonly UriKind EnumParam;

    [Parameter]
    readonly string[] ArrayParam; // TODO: value provider

    Target Types => _ => _
        .Executes(() =>
        {
            Logger.Normal($"{nameof(StringParam)} = {StringParam}");
            Logger.Normal($"{nameof(EnumParam)} = {EnumParam}");
            Logger.Normal($"{nameof(ArrayParam)} = {ArrayParam.JoinComma()}");
        });

    Target Requirements => _ => _
        .Requires(() => StringParam)
        .DependsOn(DependentTarget)
        .Executes(() =>
        {
            Logger.Normal($"String length: {StringParam.Length}");
        });

    Target DependentTarget => _ => _
        .Executes(() => Thread.Sleep(10_000));
}
