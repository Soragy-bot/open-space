using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Content.Server._NullLink.Core;
using Content.Server._NullLink.Helpers;
using Content.Server.Database;
using Content.Shared._NullLink;
using Content.Shared.NullLink.CCVar;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Starlight.NullLink;
using Starlight.NullLink.Event;

namespace Content.Server._NullLink.PlayerData;

public sealed partial class NullLinkPlayerManager : INullLinkPlayerManager
{
    public ValueTask SyncPlayTime(PlayerServerPlayTimesSyncEvent ev)
    {
        if (!_playerById.TryGetValue(ev.Player, out var playerData))
            return ValueTask.CompletedTask;

        playerData.RolePlayTimePerServer.Clear();

        foreach (var serverPlayTime in ev.ServerPlayTimes)
            playerData.RolePlayTimePerServer[serverPlayTime.Key] = serverPlayTime.Value.ToDictionary(x => x.Tracker, x => x.Time);

        SendPlayerPlayTime(playerData.Session, playerData.RolePlayTimePerServer);
        return ValueTask.CompletedTask;
    }

    private void SendPlayerPlayTime(ICommonSession session, Dictionary<string, Dictionary<string, TimeSpan>> rolePlayTimePerServer)
        => _netMgr.ServerSendMessage(new MsgUpdatePlayerPlayTime
        {
            RolePlayTimePerServer = rolePlayTimePerServer
        }, session.Channel);

    private void UpdateProject(string obj)
    {
        if (!_proto.TryIndex<ServerPlaytimeRecognitionPrototype>(obj, out var serverPlaytimeRecognition))
            return;

        _serverPlaytimeRecognition = serverPlaytimeRecognition;
    }

    private void UpdateServer(string obj) => _server = obj;
}
