using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.Logger;

partial class Build
{
    Target Parsing => _ => _
        .Executes(() =>
        {
            Info("Test projects:");
            Solution.GetProjects("*Tests")
                .ForEach(x => Normal(x)); // TODO: why no method group?

            Info("Solution folders:");
            Solution.AllSolutionFolders
                .ForEach(x => Normal(x.Name));
        });
}
