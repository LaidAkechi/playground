using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using static Nuke.Common.Logger;

[UnsetVisualStudioEnvironmentVariables]
partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>();

    // Resolved from .nuke file or --solution parameter
    [Solution(GenerateProjects = true)] readonly Solution Solution;

    // Resolved from constructor argument
    [Solution("./../../nuke-playground.sln")] readonly Solution SolutionFromPath;

    Target ViaAttributes => _ => _
        .Executes(() =>
        {
            Normal(Solution);
            Normal(SolutionFromPath);
        });
}
