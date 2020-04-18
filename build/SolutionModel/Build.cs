using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>();

    [Solution] readonly Solution SolutionFromMarkerFileOrParameter;

    [Solution("./../../nuke-playground.sln")] readonly Solution SolutionFromPath;

    // Load solution via attribute
    Target ViaAttributes => _ => _
        .Executes(() =>
        {
            Logger.Normal(SolutionFromMarkerFileOrParameter);
            Logger.Normal(SolutionFromPath);
        });

    // Get paths for project items
    Target Parsing => _ => _
        .Executes(() =>
        {
            var solution = ProjectModelTasks.ParseSolution(SolutionFromMarkerFileOrParameter);

            solution.GetProjects("*Tests")
                .ForEach(x => Logger.Normal(x)); // TODO: why no method group?

            solution.AllSolutionFolders
                .ForEach(x => Logger.Normal(x.Name));
        });

    // Create solution with solution folder and project
    Target Creation => _ => _
        .Executes(() =>
        {
            var solution = ProjectModelTasks.CreateSolution();

            var srcFolder = solution.AddSolutionFolder("src");
            var libraryProject = solution.AddProject(
                "Library",
                ProjectType.CSharpProject.FirstGuid,
                RootDirectory / "src" / "Library.csproj",
                solutionFolder: srcFolder);
        });

    // Add project to existing solution
    Target Modifications => _ => _
        .Executes(() =>
        {
            var solution = ProjectModelTasks.ParseSolution(RootDirectory / "nuke-playground.sln");

            solution.AddProject(
                "NonExistentProject",
                ProjectType.CSharpProject.FirstGuid,
                RootDirectory / "src" / "NonExistentProject.csproj");

            solution.Save();
        });
}
