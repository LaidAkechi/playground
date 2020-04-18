using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Nuke.Common;
using Nuke.Common.Utilities;
// ReSharper disable InconsistentNaming

partial class Build
{
    [Parameter("")]
    readonly string StringParam;

    [Parameter]
    readonly UriKind EnumParam;

    [Parameter]
    readonly string[] ArrayParam; // TODO: value provider

    Target Parameters_Types => _ => _
        .Executes(() =>
        {
            Logger.Normal($"{nameof(StringParam)} = {StringParam}");
            Logger.Normal($"{nameof(EnumParam)} = {EnumParam}");
            Logger.Normal($"{nameof(ArrayParam)} = {ArrayParam.JoinComma()}");
        });

    Target Parameter_Requirements => _ => _
        .Requires(() => StringParam)
        .DependsOn(Parameter_Requirements_DependentTarget)
        .Executes(() =>
        {
            Logger.Normal($"String length: {StringParam.Length}");
        });

    Target Parameter_Requirements_DependentTarget => _ => _
        .Executes(() => Thread.Sleep(10_000));
}
