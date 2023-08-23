using Lobby.Grpc;

namespace Lobby.Game
{
    public interface IMatch
    {
        int GetGameNr();
        int GetPlayersNr();
        bool AddPlayer(Peer peer);
        bool IsFull();
        List<Peer> GetPlayers();
        int GetDealerId();
    }
}