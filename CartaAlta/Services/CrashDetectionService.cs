using CartaAlta.Grpc;

namespace CartaAlta.Services
{
    public class CrashDetectionService
    {
        private int _takeControlRequestReceived;
        private Timer _timer;
        private int _defaultTimeout;
        private string _myName;

        public string LastPlayer { get; private set; }

        public CrashDetectionService()
        {
            _myName = DotNetEnv.Env.GetString("NODE_NAME");
            _defaultTimeout = Utils.Constants.DEFAULT_TIMEOUT;
            _timer = new Timer(SendTakeControlRequest, null, TimeSpan.FromSeconds(_defaultTimeout), TimeSpan.FromSeconds(_defaultTimeout));
            _takeControlRequestReceived = 0;
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
            // controllo se sono rimasto solo, in tal caso nessuno mi invierà una take control request quindi effettuo il turno
            if (GetActivePlayersName().Count() < 1)
            {
                ServicePool.GameEngine.MakeTurnAndPass();
                return;
            }

            // invia all'ultimo giocatore ATTIVO una richiesta per prendere il controllo del gioco
            // se sono io non faccio niente in quanto aspetto qualcuno mi invio una take control request
            var lastActivePlayer = GetLastActivePlayerName();
            if (!(lastActivePlayer.Name == _myName))
                ServicePool.P2PService.SendTakeControlRequest(lastActivePlayer);
        }

        private Peer GetLastActivePlayerName()
        {
            var found = false;
            var nr_checked = 0;
            var peersPossiblyOnline = ServicePool.DbService.PeerDb.GetAllToList();
            peersPossiblyOnline.RemoveAll(peer => peer.Name == _myName);

            while(!found || nr_checked >= peersPossiblyOnline.Count)
            {
                var isOnline = P2P.P2PService.Ping(peersPossiblyOnline.ElementAt(nr_checked));
                if (isOnline) found = true;
                else nr_checked++;
            }

            return found ? peersPossiblyOnline.ElementAt(nr_checked) : ServicePool.DbService.PeerDb.GetByName(_myName);
        }

        private IEnumerable<string> GetActivePlayersName()
        {
            var peersPossiblyOnline = ServicePool.DbService.PeerDb.GetAllToList();
            foreach (var peer in peersPossiblyOnline)
            {
                if (P2P.P2PService.Ping(peer))
                    yield return peer.Name;
            }
        }

        private bool IsDealerOnline()
        {
            return GetActivePlayersName().Contains(ServicePool.GameEngine.DealerName);
        }

        internal void UpdateState()
        {
            _takeControlRequestReceived = 0;
            RestartTimer();
        }

        internal void ProcessTakeControlRequest(GameServiceRequest request)
        {
            _takeControlRequestReceived += 1;
            var nrOnlinePeers = GetActivePlayersName().Count();
            if (!(_takeControlRequestReceived >= nrOnlinePeers)) return;
            if (IsDealerOnline())
                ServicePool.GameEngine.MakeTurnAndPass();
            else
            {
                ServicePool.GameEngine.IsDealer = true;
                ServicePool.GameEngine.DealerTurn();
            }
        }
    }
}
