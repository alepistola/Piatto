using Piatto.Grpc;

namespace Piatto.Services
{
    public class CrashDetectionService
    {
        private Timer _timer;
        private int _defaultTimeout;
        private string _myName;

        public string LastPlayer { get; private set; }

        public CrashDetectionService()
        {
            _myName = DotNetEnv.Env.GetString("NODE_NAME");
            _defaultTimeout = Utils.Constants.DEFAULT_TIMEOUT;
            _timer = new Timer(SendTakeControlRequest, null, TimeSpan.FromSeconds(_defaultTimeout), TimeSpan.FromSeconds(_defaultTimeout));
        }

        private void StartTimer()
        {
            _timer = new Timer(SendTakeControlRequest, null, TimeSpan.FromSeconds(_defaultTimeout), TimeSpan.FromSeconds(_defaultTimeout));
        }

        public void Stop()
        {
            Console.WriteLine("... Stopping crash detection service");
            _timer.Dispose();
        }

        public void RestartTimer()
        {
            _timer.Dispose();
            StartTimer();
        }

        private void SendTakeControlRequest(object? state)
        {
            var onlinePlayersName = GetActivePlayersName();

            // controllo se vi è stato un almeno un crash
            if (onlinePlayersName.Count() == ServicePool.DbService.PeerDb.GetAll().Count() && onlinePlayersName.Count() > 1)
                return;


            // controllo se sono rimasto solo, in tal caso nessuno mi invierà una take control request quindi effettuo il turno
            if (onlinePlayersName.Count() <= 1)
            {
                ServicePool.GameEngine.EndGameCauseCrashed();
                return;
            }

            var lastActivePlayer = GetLastActivePlayerName(onlinePlayersName);

            // se non sono il giocatore che deve passare il turno ritorno
            if (!(lastActivePlayer.Name == _myName))
                return;

            if (IsDealerOnline())
                ServicePool.GameEngine.PassTurn(false);
            else
                ServicePool.GameEngine.PassTurn(true);
        }


        private Peer GetLastActivePlayerName(IEnumerable<string> onlinePlayers)
        {
            var lastActivePlayer = ServicePool.DbService.MoveDb.GetLast(onlinePlayers.Count())
                                                                    .Select(mv => mv.Author).
                                                                    Intersect(onlinePlayers)
                                                                    .First();

            return ServicePool.DbService.PeerDb.GetByName(lastActivePlayer);
        }



        private IEnumerable<string> GetActivePlayersName()
        {
            var peersPossiblyOnline = ServicePool.DbService.PeerDb.GetAllToList();
            List<string> playersName = new();
            foreach (var peer in peersPossiblyOnline)
            {
                if (ServicePool.P2PService.Ping(peer))
                    playersName.Add(peer.Name);
            }
            return playersName;
        }

        private bool IsDealerOnline()
        {
            return GetActivePlayersName().Contains(ServicePool.GameEngine.DealerName);
        }

        internal void UpdateState()
        {
            RestartTimer();
        }
    }
}
