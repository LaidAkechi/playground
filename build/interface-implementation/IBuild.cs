using Nuke.Common;
using static Nuke.Common.Execution.InjectionUtility;

interface IBuild : IGitVersioned, IHasSolution
{
    [Parameter] Configuration Configuration => GetInjectionValue(() => Configuration);

    Target Restore => _ => _
        .Executes(() =>
        {
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
        });
}
