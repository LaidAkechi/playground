using Nuke.Common;

interface IPublishNuGet : IBuild, IHasPackageOutput
{
    Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
        });

    Target Publish => _ => _
        .DependsOn(Pack)
        .Executes(() =>
        {
        });
}