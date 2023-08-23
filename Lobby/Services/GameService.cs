using Grpc.Net.Client;
using Lobby.Game;
using Lobby.Game.DTO;
using Lobby.Grpc;
using Lobby.Utils;

namespace Lobby.Services
{
    public class GameService : IGameService
    {
        private List<IMatch> _matches;

        public GameService()
        {
            _matches = new List<IMatch>();
            CreateDefaultMatch();
        }

        private IMatch CreateDefaultMatch()
        {
            var match = new Match(MatchSettingsFactory.generateFromEnv());
            _matches.Add(match);
            return match;
        }

        public IMatch AddPlayerToMatch(Peer peer)
        {
            // search for open match
            // if exists add player and return the match
            // else create a new match, add the player and return the match
            var openMatch = SearchForOpenMatch();
            var match = openMatch ?? CreateMatch(null);
            match.AddPlayer(peer);
            Task.Run(CheckAndNotify);
            return match;
        }

        private IMatch? SearchForOpenMatch()
        {
            return _matches.Find(match => !match.IsFull());
        }

        private IMatch CreateMatch(MatchSettings? matchSetting)
        {
            IMatch match = (matchSetting != null) ? new Match(matchSetting) : CreateDefaultMatch();
            _matches.Add(match);
            return match;
        }

        public void CheckAndNotify()
        {
            Parallel.ForEach(_matches, match =>
            {
                Console.WriteLine("Checking game nr. {0} ({1})", match.GetGameNr(), ExtensionMethods.Extensions.StringJoin(match.GetPlayers().Select(p => p.Address).ToArray(), ", "));
                if (match.IsFull()) {
                Thread.Sleep(5000);
                    NotifyPartecipants(match);
                RemoveMatch(match);
            }
            });
        }

        private void RemoveMatch(IMatch match)
        {
            _matches.RemoveAll(m => m.GetGameNr == match.GetGameNr);
        }

        private static void NotifyPartecipants(IMatch match)
        {
            foreach (var player in match.GetPlayers())
            {
                Console.WriteLine("-- Sending starting game notify to {0}", player.Address);
                GrpcChannel channel = GrpcChannel.ForAddress(player.Address);
                var matchmakingService = new Matchmaking.MatchmakingClient(channel);
                try
                {
                    var startMatchInfo = new StartMatchInfo();
                    startMatchInfo.PeerList.AddRange(match.GetPlayers());
                    startMatchInfo.Dealer = (match.GetDealerId() == player.Id);

                    var response = matchmakingService.StartMatch(startMatchInfo);
                    if (response.Status == true)
                        Console.WriteLine("--- Done: received ack from {0}", player.Address);
                    else
                        Console.WriteLine("--- Starting game notify sent but ack not received from {0}", player.Address);
                }
                catch
                {
                    Console.WriteLine("--- Failed to send to {0}", player.Address);
                }
            }


            /*
            Parallel.ForEach(match.GetPlayers(), player =>
            {
                
            });
            */
        }
    }
}
