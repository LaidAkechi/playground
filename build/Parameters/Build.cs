using System;
using System.Threading;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Utilities;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Print);

    [Parameter("String parameter. Needs to be quoted.")]
    readonly string StringParam;

    [Parameter("Switch parameter.")]
    readonly bool? BoolParam;

    [Parameter("Enum parameter with default value.")]
    readonly UriKind EnumParam;

    [Parameter("Array parameter with custom separator", Separator = "+")]
    readonly string[] ArrayParam; // TODO: value provider

    Target Print => _ => _
        .Executes(() =>
        {
            Logger.Normal($"{nameof(StringParam)} = {StringParam ?? "<null>"}");
            Logger.Normal($"{nameof(BoolParam)} = {(BoolParam.HasValue ? BoolParam.Value.ToString() : "<null>")}");
            Logger.Normal($"{nameof(EnumParam)} = {EnumParam}");
            Logger.Normal($"{nameof(ArrayParam)} = {ArrayParam?.JoinComma() ?? "<null>"}");
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
