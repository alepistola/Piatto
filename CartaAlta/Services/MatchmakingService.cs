using CartaAlta.Grpc;
using Grpc.Net.Client;

namespace CartaAlta.Services
{
    public class MatchmakingService
    {
        private readonly string _lobbyAddress;
        private readonly string _nodeAddress;
        private readonly string _nodeName;
        private readonly int _nodeId;

        public MatchmakingService()
        {
            _lobbyAddress = DotNetEnv.Env.GetString("LOBBY_ADDRESS");
            _nodeAddress = DotNetEnv.Env.GetString("NODE_ADDRESS");
            _nodeId = DotNetEnv.Env.GetInt("NODE_ID");
            _nodeName = DotNetEnv.Env.GetString("NODE_NAME");
        }

        public void Start()
        {
            Console.WriteLine("... Starting matchmaking service");
            GrpcChannel channel = GrpcChannel.ForAddress(_lobbyAddress);
            var matchmakingService = new Matchmaking.MatchmakingClient(channel);
            try
            {
                Console.WriteLine("... Node {0} ({1}) now sending a register for a match request", _nodeId.ToString(), _nodeAddress);
                var response = matchmakingService.RegisterForMatch(new Peer { Address = _nodeAddress, Id = _nodeId, Name = _nodeName });
                Console.WriteLine("... Correctly registered for game nr. {0} ({1}/4)", response.GameNumber, response.PlayerNumber);
            }
            catch(Exception ex) {
                Console.WriteLine("... Connection problem while connecting to lobby, please retry!");
                Console.WriteLine(ex.ToString());
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}