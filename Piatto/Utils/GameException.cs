namespace Piatto.Utils
{
    [Serializable]
    public class GameException : Exception
    {
        public string? ExceptionMessage { get; }
        public bool IsTheOnlyPlayer { get; }

        public GameException() { }

        public GameException(string message)
            : base(message) { }

        public GameException(string message, Exception inner)
            : base(message, inner) { }

        public GameException(string message, string exceptionMessage)
            : this(message)
        {
            ExceptionMessage = exceptionMessage;
        }

        public GameException(string message, bool isTheOnlyPlayer) : base(message)
        {
            IsTheOnlyPlayer = isTheOnlyPlayer;
        }
    }
}
