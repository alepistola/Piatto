using Lobby.Game;
using Lobby.Grpc;

namespace Lobby.Services
{
    public interface IGameService
    {
        public IMatch AddPlayerToMatch(Peer peer);
        public void CheckAndNotify();
        // public IMatch CreateMatch(MatchSettings matchSetting);
    }
}
