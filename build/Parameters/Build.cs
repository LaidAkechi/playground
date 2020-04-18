using System;
using System.Threading;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Utilities;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Print);

    [Parameter("Just a string parameter. Needs to be quoted.")]
    readonly string StringParam;

    [Parameter]
    readonly UriKind EnumParam;

    [Parameter]
    readonly string[] ArrayParam; // TODO: value provider

    Target Print => _ => _
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
