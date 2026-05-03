using System.Linq;
using Content.Server._NullLink.PlayerData;
using Content.Shared._NullLink;
using Robust.Server.Player;
using Robust.Shared.Player;

namespace Content.Server._NullLink.PlayerData;

public sealed class PlayerRolesReqManager : SharedPlayerRolesReqManager
{
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly INullLinkPlayerManager _playerManager = default!;

    public override bool IsAllRolesAvailable(EntityUid uid)
        => _player.TryGetSessionByEntity(uid, out var session)
            && IsAllRolesAvailable(session);

    public override bool IsAllRolesAvailable(ICommonSession session)
        => AllRoles is not null
            && _playerManager.TryGetPlayerData(session.UserId, out var data)
            && AllRoles.Roles.Any(data.Roles.Contains);

    public override bool IsAnyRole(ICommonSession session, ulong[] roles)
        => _playerManager.TryGetPlayerData(session.UserId, out var data)
            && roles.Any(data.Roles.Contains);

    public override bool IsMentor(EntityUid uid)
        => _player.TryGetSessionByEntity(uid, out var session)
            && IsMentor(session);

    public override bool IsMentor(ICommonSession session)
        => _mentorReq is not null
            && _playerManager.TryGetPlayerData(session.UserId, out var data)
            && _mentorReq.Roles.Any(data.Roles.Contains);

    public override bool IsPeacefulBypass(EntityUid uid)
        => _player.TryGetSessionByEntity(uid, out var session)
            && IsPeacefulBypass(session);

    private bool IsPeacefulBypass(ICommonSession session)
        => _peacefulBypass is not null
            && _playerManager.TryGetPlayerData(session.UserId, out var data)
            && _peacefulBypass.Roles.Any(data.Roles.Contains);
}
