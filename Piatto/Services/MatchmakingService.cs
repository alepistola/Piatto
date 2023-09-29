using Piatto.Grpc;
using Grpc.Net.Client;

namespace Piatto.Services
{
    public class MatchmakingService
    {
        private readonly string _lobbyAddress;
        private readonly string _nodeAddress;
        private readonly string _nodeName;

        public MatchmakingService()
        {
            _lobbyAddress = DotNetEnv.Env.GetString("LOBBY_ADDRESS");
            _nodeAddress = DotNetEnv.Env.GetString("NODE_ADDRESS");
            _nodeName = DotNetEnv.Env.GetString("NODE_NAME");
        }

        public void Start()
        {
            Console.WriteLine("... Starting matchmaking service");
            GrpcChannel channel = GrpcChannel.ForAddress(_lobbyAddress);
            var matchmakingService = new Matchmaking.MatchmakingClient(channel);
            try
            {
                Console.WriteLine("... Node {0} ({1}) now sending a register for a match request", _nodeName, _nodeAddress);
                var response = matchmakingService.RegisterForMatch(new Peer { Address = _nodeAddress, Name = _nodeName });
                Console.WriteLine("... Correctly registered for game nr. {0} ({1})", response.GameNumber, response.PlayerNumber);
            }
            catch(Exception ex) {
                Console.WriteLine("... Connection problem while connecting to lobby, it may be offline!");
                Console.WriteLine("... Try again restarting the application");
                Environment.Exit(0);
                // Console.WriteLine(ex.ToString());
            }
        }

        public void Stop()
        {
            Console.WriteLine("... Stopping Matchmaking service");
        }
    }
}