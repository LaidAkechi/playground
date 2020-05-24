using Nuke.Common;

public interface IAnnounce
{
    Target Announce => _ => _
        .Executes(() =>
        {
            Logger.Info(Message);
        });

    string Message { get; }
}
