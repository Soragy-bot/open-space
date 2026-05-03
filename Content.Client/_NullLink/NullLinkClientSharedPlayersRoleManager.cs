using Robust.Client.Player;
using Robust.Shared.Player;
using SharedPlayerData = Content.Shared.Starlight.PlayerData;

namespace Content.Client._NullLink;

public sealed class NullLinkClientSharedPlayersRoleManager : Content.Shared.Starlight.ISharedPlayersRoleManager
{
    [Dependency] private readonly IPlayerManager _player = default!;

    private readonly SharedPlayerData _localData = new();

    public SharedPlayerData? GetPlayerData(EntityUid uid)
        => _player.LocalEntity == uid ? _localData : null;

    public SharedPlayerData? GetPlayerData(ICommonSession session)
        => _player.LocalSession == session ? _localData : null;
}
