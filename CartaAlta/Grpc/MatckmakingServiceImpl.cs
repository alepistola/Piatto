using CartaAlta.Services;
using Grpc.Core;

namespace CartaAlta.Grpc
{
    public class MatckmakingServiceImpl : Matchmaking.MatchmakingBase
    {
        public override Task<StartMatchAck> StartMatch(StartMatchInfo matchInfo, ServerCallContext context)
        {
            Console.WriteLine("-> Game now starting...");
            Console.WriteLine("-> Peers: {0}", Utils.Extensions.StringJoin(matchInfo.PeerList.Select(p => p.Address).ToArray(), ", "));
            Console.WriteLine("-> Dealer: {0}", matchInfo.Dealer.ToString());

            var peerList = matchInfo.PeerList.ToList<Peer>();

            ServicePool.DbService.PeerDb.AddBulk(peerList);
            ServicePool.InitializeWithPeers(peerList, matchInfo.Dealer);

            Task.Run(() => ServicePool.GameEngine.DealerTurn());

            return Task.FromResult(new StartMatchAck
            {
                Status = true
            });
        }
    }
}
