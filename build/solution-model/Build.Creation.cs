using Nuke.Common;
using Nuke.Common.ProjectModel;

partial class Build
{
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
}
