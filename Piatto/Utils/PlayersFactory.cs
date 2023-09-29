using Piatto.Game;
using Piatto.Grpc;

namespace Piatto.Utils
{
    public static class PlayersFactory
    {
        // TODO 
        // GeneratePlayers(matchSettings)

        public static Dictionary<string, Player> GeneratePlayers(List<Peer> peers)
        {
            var defaultBalance = Constants.DEFAULT_BALANCE;
            var minBet = Constants.MIN_BET;

            var playersDict = new Dictionary<string, Player>();
            foreach (var peer in peers)
                playersDict[peer.Name] = GenerateDefaultPlayer(peer.Name, defaultBalance, minBet);
            
            return playersDict;
        }

        private static Player GenerateDefaultPlayer(string name, double defaultBalance, double minBet)
        {
            return new Player(name, defaultBalance, minBet);
        }
    }
}
