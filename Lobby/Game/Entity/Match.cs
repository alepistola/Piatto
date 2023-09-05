using Lobby.Game.DTO;
using Lobby.Grpc;

namespace Lobby.Game
{
    public class Match : IMatch
    {
        private readonly MatchSettings _matchSettings;
        private List<Peer> _players;
        private int _dealerId;

        public Match(MatchSettings settings)
        {
            _players = new List<Peer>();
            _matchSettings = settings;
            _dealerId = -1;
        }

        public bool AddPlayer(Peer peer) {
            if (IsFull()) return false;
            _players.Add(peer);
            return true;
        }

        public string GetDealerName()
        {
            // just 4 debug
            return "Ale";

            /*
            if (_dealerId == -1) {
                var rnd = new Random();
                _dealerId = _players.ElementAt(rnd.Next(0, GetPlayersNr())).Id;
            }
            return _dealerId;
            */
        }

        public int GetGameNr() {
            return _matchSettings.GameNr;
        }

        public List<Peer> GetPlayers()
        {
            return _players;
        }

        public int GetPlayersNr() {
            return _players.Count;
        }

        public bool IsFull() { return (_players.Count >= _matchSettings.MaxPlayers); }
    }
}
