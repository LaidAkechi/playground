using Nuke.Common;

interface IPublish : IBuild, IHasPackageOutput
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