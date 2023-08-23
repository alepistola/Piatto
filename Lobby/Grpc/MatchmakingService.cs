using Grpc.Core;
using Lobby.Services;

namespace Lobby.Grpc
{
    public class MatchmakingService : Matchmaking.MatchmakingBase
    {
        private readonly ILogger<MatchmakingService> _logger;
        private readonly IGameService _gameService;

        public MatchmakingService(ILogger<MatchmakingService> logger, IGameService gameService)
        {
            _logger = logger;
            _gameService = gameService;
        }

        public override Task<RegisterReply> RegisterForMatch(Peer peer, ServerCallContext context)
        {
            Console.WriteLine("-> Received register request for a match from Peer {0}-{1} ({2})", peer.Id, peer.Name ,peer.Address);
            var match =  _gameService.AddPlayerToMatch(peer);
            return Task.FromResult(new RegisterReply
            {
                GameNumber = match.GetGameNr(),
                PlayerNumber = match.GetPlayersNr()
            });
        }
    }
}