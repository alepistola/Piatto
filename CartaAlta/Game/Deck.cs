using CartaAlta.Grpc;

namespace CartaAlta.Game
{
    public class Deck
    {
        private readonly List<Card> _cards;

        public Deck()
        {
            _cards = CreateDeck();
        }

        public Deck(List<Card> cards) => _cards = cards;
        

        private static List<Card> CreateDeck()
        {
            List<Card> newDeck = new();
            foreach (Seme seme in Enum.GetValues(typeof(Seme)))
            {
                for (int valore = 1; valore <= 10; valore++)
                {
                    newDeck.Add(new Card { Seme = seme,  Valore = valore });
                }
            }
            return newDeck;
        }

        public void Shuffle()
        {
            Random random = new();
            int n = _cards.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                (_cards[n], _cards[k]) = (_cards[k], _cards[n]);
            }
        }

        public Card Draw()
        {
            if (_cards.Count == 0)
                throw new InvalidOperationException("Il mazzo è vuoto");

            Card cartaPescata = _cards[0];
            _cards.RemoveAt(0);
            return cartaPescata;
        }

        public List<Card> GetCards() => _cards;
        
        /*
        private static List<Card> GenerateDeckFromGrpcClass(List<Grpc.Card> cards)
        {
            var deck = new List<Card>();
            for (int i = 0; i < cards.Count; i++)
            {
                var card = new Card
                (
                    seme: ((int)cards[i].Seme),
                    valore: (cards[i].Valore)
                );
                deck.Add(card);
            }
            return deck;
        }
        */
    }
}
