using CartaAlta.Grpc;
using CartaAlta.P2P;

namespace CartaAlta.Services
{
    public static class ServicePool
    {
        public static MatchmakingService MatchmakingService { set; get; }
        public static GameEngine GameEngine { set; get; }
        public static DbService DbService { set; get; }
        public static P2PService P2PService { get; set; }

        public static CrashDetectionService CrashDetectionService { get; set; }


        public static void Add(MatchmakingService matchmakingService, DbService dbService, CrashDetectionService crashDetectionService)
        {
            MatchmakingService = matchmakingService;
            DbService = dbService;
            CrashDetectionService = crashDetectionService;
        }

        public static void Start()
        {
            MatchmakingService.Start();
            DbService.Start();
        }

        public static void Stop()
        {
            //stop when application exit
            P2PService.Stop();
            DbService.Stop();
            CrashDetectionService.Stop();
        }

        public static void InitializeWithPeers(List<Peer> peerList, bool isDealer)
        {
            GameEngine = new GameEngine(peerList, isDealer);
            P2PService = new P2PService();
            CrashDetectionService.Start();
        }
    }
}