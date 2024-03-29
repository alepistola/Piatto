﻿using Piatto.Grpc;
using Piatto.P2P;

namespace Piatto.Services
{
    public static class ServicePool
    {
        public static MatchmakingService MatchmakingService { set; get; }
        public static GameEngine GameEngine { set; get; }
        public static DbService DbService { set; get; }
        public static P2PService P2PService { get; set; }

        public static CrashDetectionService CrashDetectionService { get; set; }


        public static void Add(MatchmakingService matchmakingService, DbService dbService)
        {
            MatchmakingService = matchmakingService;
            DbService = dbService;
        }

        public static void Start()
        {
            MatchmakingService.Start();
            DbService.Start();
        }

        public static void Stop()
        {
            try
            {
                //stop when application exit
                P2PService.Stop();
                DbService.Stop();
                MatchmakingService.Stop();
                CrashDetectionService.Stop();
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("One of the services it may have raised an exception during the stopping process..");
            }
            
        }

        public static void InitializeWithPeers(List<Peer> peerList, string dealerName)
        {
            GameEngine = new GameEngine(peerList, dealerName);
            P2PService = new P2PService();
            CrashDetectionService = new CrashDetectionService();
        }
    }
}