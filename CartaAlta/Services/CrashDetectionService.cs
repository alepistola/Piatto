using CartaAlta.Grpc;
using CartaAlta.Utils;

namespace CartaAlta.Services
{
    public class CrashDetectionService
    {
        private Timer _timer;
        private readonly int _timeout = Constants.DEFAULT_MOVE_TIMEOUT;
        private string _currentDealer;
        private readonly string _nodeAddress;
        private readonly string _nodeName;

        public CrashDetectionService()
        {
            Console.WriteLine("... Crash detection service is starting");
            _nodeAddress = DotNetEnv.Env.GetString("NODE_ADDRESS");
            _nodeName = DotNetEnv.Env.GetString("NODE_NAME");
            Console.WriteLine("...... Crash detection service is ready");
        }

        #region Timer
        public Task Start()
        {
            _timer = new Timer(CheckAndSignalDealerCrash, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(_timeout));

            return Task.CompletedTask;
        }

        public Task Stop()
        {
            Console.WriteLine("... Crash detection service is stopping...");

            _timer?.Change(Timeout.Infinite, 0);
            StopTimer();

            Console.WriteLine("... Crash detection service has been disposed");
            return Task.CompletedTask;
        }

        private void StopTimer() => _timer.Dispose();

        private void StartTimer()
        {
            _timer = new Timer(CheckAndSignalDealerCrash, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(_timeout));
        }

        public void Restart()
        {
            StopTimer();
            StartTimer();
        }

        #endregion

        private void CheckAndSignalDealerCrash(object? state)
        {

        }

        private bool PingDealer()
        {
            throw new NotImplementedException();
        }

        public bool UpdateState(Move mv)
        {
            throw new NotImplementedException();
        }

        
    }
}
