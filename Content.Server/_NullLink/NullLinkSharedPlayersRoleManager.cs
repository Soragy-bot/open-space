using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using SharedPlayerData = Content.Shared.Starlight.PlayerData;

namespace Content.Server._NullLink;

public sealed class NullLinkSharedPlayersRoleManager : Content.Shared.Starlight.ISharedPlayersRoleManager, IPostInjectInit
{
    [Dependency] private readonly IPlayerManager _player = default!;

    private readonly Dictionary<Guid, SharedPlayerData> _data = new();

    void IPostInjectInit.PostInject()
    {
        _player.PlayerStatusChanged += OnPlayerStatusChanged;
    }

    private void OnPlayerStatusChanged(object? sender, SessionStatusEventArgs e)
    {
        switch (e.NewStatus)
        {
            case SessionStatus.Connected:
                _data.TryAdd(e.Session.UserId, new SharedPlayerData());
                break;
            case SessionStatus.Disconnected:
                _data.Remove(e.Session.UserId);
                break;
        }
    }

    public SharedPlayerData? GetPlayerData(EntityUid uid)
    {
        if (!_player.TryGetSessionByEntity(uid, out var session))
            return null;
        return GetPlayerData(session);
    }

    public SharedPlayerData? GetPlayerData(ICommonSession session)
    {
        _data.TryGetValue(session.UserId, out var data);
        return data;
    }
}
