﻿using Lobby.Game;
using Lobby.Grpc;

namespace Lobby.Services
{
    public interface IGameService
    {
        public IMatch AddPlayerToMatch(Peer peer);
        public void CheckAndNotify(IMatch match);
        // public IMatch CreateMatch(MatchSettings matchSetting);
    }
}
