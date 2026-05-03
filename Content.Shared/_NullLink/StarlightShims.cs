using Robust.Shared.Player;
using Robust.Shared.Serialization;

namespace Content.Shared.Starlight;

// Shims required by _NullLink that were originally defined in _Starlight.
// These live here to avoid pulling in the full _Starlight module.

[Serializable, NetSerializable]
public sealed class PlayerData
{
    public string? Title;
    public string? GhostTheme;
    public Color GhostThemeColor = Color.White;

    [Obsolete("Use ISharedNullLinkPlayerResourcesManager to access resources")]
    public Dictionary<string, double> Resources = [];
}

public interface ISharedPlayersRoleManager
{
    PlayerData? GetPlayerData(EntityUid uid);
    PlayerData? GetPlayerData(ICommonSession session);
}
