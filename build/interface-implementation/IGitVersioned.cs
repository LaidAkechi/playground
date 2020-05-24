using Nuke.Common.Execution;
using Nuke.Common.Tools.GitVersion;

interface IGitVersioned
{
    [GitVersion] GitVersion GitVersion => InjectionUtility.GetInjectionValue(() => GitVersion);
}