using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;

interface IHasSolution
{
    [Solution] Solution Solution => InjectionUtility.GetInjectionValue(() => Solution);
}