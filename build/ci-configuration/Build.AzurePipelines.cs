using Nuke.Common.CI.AzurePipelines;

[AzurePipelines(
    AzurePipelinesImage.Ubuntu1604,
    AzurePipelinesImage.Ubuntu1804,
    InvokedTargets = new[] {nameof(Test)},
    // Target without dedicated agent execution
    NonEntryTargets = new[] {nameof(Compile)})]
partial class Build
{
}
