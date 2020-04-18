using Nuke.Common;

partial class Build
{
    Target GraphModel_Clean => _ => _
        .Before(GraphModel_Restore);

    Target GraphModel_Restore => _ => _;

    Target GraphModel_Compile => _ => _
        .DependsOn(GraphModel_Restore);

    Target GraphModel_Pack => _ => _
        .DependsOn(GraphModel_Compile);

    Target GraphModel_Test => _ => _
        .DependsOn(GraphModel_Compile);

    Target GraphModel_Publish => _ => _
        .DependsOn(GraphModel_Pack);

    Target GraphMode_Announce => _ => _
        .TriggeredBy(GraphModel_Publish);
}
