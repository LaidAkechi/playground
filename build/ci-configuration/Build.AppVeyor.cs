using Nuke.Common.CI.AppVeyor;

[AppVeyor(
    AppVeyorImage.UbuntuLatest,
    AppVeyorImage.VisualStudioLatest,
    InvokedTargets = new[] {nameof(Test)})]
partial class Build
{
}
