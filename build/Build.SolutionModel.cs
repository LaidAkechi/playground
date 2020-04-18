using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities.Collections;
// ReSharper disable InconsistentNaming

partial class Build
{
    [Solution] readonly Solution SolutionFromMarkerFileOrParameter;

    [Solution("nuke-playground.sln")] readonly Solution SolutionFromPath;

    // Load solution via attribute
    Target SolutionModel_Attribute => _ => _
        .Executes(() =>
        {
            Logger.Normal(SolutionFromMarkerFileOrParameter);
            Logger.Normal(SolutionFromPath);
        });

    // Get paths for project items
    Target SolutionModel_Parsing => _ => _
        .Executes(() =>
        {
            var solution = ProjectModelTasks.ParseSolution(RootDirectory / "nuke-playground.sln");

            solution.GetProjects("*Tests")
                .ForEach(x => Logger.Normal(x)); // TODO: why no method group?

            solution.AllSolutionFolders
                .ForEach(Logger.Normal);
        });

    // Create solution with solution folder and project
    Target SolutionModel_Creation => _ => _
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
    Target SolutionModel_Modification => _ => _
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
