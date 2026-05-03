using Content.Shared._NullLink;
using Robust.Shared.Network;

namespace Content.Client._NullLink;

public sealed class NullLinkPlayTimeManager : INullLinkPlayTimeManager
{
    [Dependency] private readonly IClientNetManager _netMgr = default!;

    private Dictionary<string, Dictionary<string, TimeSpan>> _serverPlayTimes = new();

    public void Initialize()
    {
        _netMgr.RegisterNetMessage<MsgUpdatePlayerPlayTime>(OnPlayTimeUpdated);
    }

    private void OnPlayTimeUpdated(MsgUpdatePlayerPlayTime msg)
    {
        _serverPlayTimes = msg.RolePlayTimePerServer;
    }

    public TimeSpan GetPlayTime(string server, Guid _, string tracker)
        => _serverPlayTimes.TryGetValue(server, out var serverData)
            && serverData.TryGetValue(tracker, out var time)
                ? time
                : TimeSpan.Zero;
}
