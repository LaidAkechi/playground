using Nuke.Common;
using Nuke.Common.ProjectModel;

partial class Build
{
    Target Modifications => _ => _
        .Executes(() =>
        {
            Solution.AddProject(
                "NonExistent",
                typeId: ProjectType.CSharpProject.FirstGuid,
                path: RootDirectory / "NonExistent.csproj");

            Solution.Save();
        });
}
