namespace CartaAlta.Services
{
    public class CrashDetectionService
    {
        private Timer _timer;
        private int _defaultTimeout;

        public CrashDetectionService()
        {
            _defaultTimeout = Utils.Constants.DEFAULT_TIMEOUT;
        }

        public void Start()
        {
            _timer = new Timer(CheckPossibleCrash, null, TimeSpan.FromSeconds(_defaultTimeout), TimeSpan.FromSeconds(_defaultTimeout));
        }

        public void Stop()
        {
            Console.WriteLine("... Stopping crash detection service");
            _timer.Dispose();
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        private void CheckPossibleCrash(object? state)
        {
            Console.WriteLine($"Sono passati {_defaultTimeout} secondi");
        }
    }
}
