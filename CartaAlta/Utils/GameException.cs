namespace CartaAlta.Utils
{
    [Serializable]
    public class GameException : Exception
    {
        public string? exceptionMessage { get; }

        public GameException() { }

        public GameException(string message)
            : base(message) { }

        public GameException(string message, Exception inner)
            : base(message, inner) { }

        public GameException(string message, string studentName)
            : this(message)
        {
            exceptionMessage = studentName;
        }
    }
}
