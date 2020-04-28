using System;
using System.Threading;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Utilities;
using static Nuke.Common.Logger;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Print);

    [Parameter] readonly string StringParam;
    [Parameter] readonly bool? BoolParam;
    [Parameter] readonly UriKind EnumParam;

    [Parameter(Separator = "+")] readonly string[] ArrayParam; // TODO: value provider

    Target Print => _ => _
        .Executes(() =>
        {
            Normal($"{nameof(StringParam)} = {StringParam ?? "<null>"}");
            Normal($"{nameof(BoolParam)} = {(BoolParam.HasValue ? BoolParam.Value.ToString() : "<null>")}");
            Normal($"{nameof(EnumParam)} = {EnumParam}");
            Normal($"{nameof(ArrayParam)} = {ArrayParam?.JoinComma() ?? "<null>"}");
        });
}
