using CartaAlta.Game;
using CartaAlta.Grpc;
using CartaAlta.Utils;

namespace CartaAlta.Services
{
    public class GameEngine
    {
        public bool IsDealer { get; set; }
        private int TurnNr { get; set; }
        private Dictionary<string, Player> _players;
        private Deck _deck;
        private double _piatto;
        private readonly string _myName;


        public GameEngine(List<Peer> peerList, bool isDealer)
        { 
            _players = PlayersFactory.GeneratePlayers(peerList);
            IsDealer = isDealer;
            _deck = new Deck();
            _piatto = 0;
            _myName = DotNetEnv.Env.GetString("NODE_NAME");
            TurnNr = 0;
        }

        public void SetSynDeck(List<Card> cards) => _deck = new Deck(cards);

        public void DealerTurn()
        {
            if (!IsDealer)
            {
                Console.WriteLine("You're not the dealer, just wait for your turn..");
                return;
            }
            Thread.Sleep(10000);

            if (!(TurnNr < 2))
            {
                ChangeDealer();
                return;
            }

            if (TurnNr == 0)
            {
                ShuffleDeck();
                AskInitialBet();
            }

            MakeTurn();

            TurnNr++;
            PassTurn(false);
        }

        private void PrintGameState()
        {
            var lastMoveNr = ServicePool.DbService.MoveDb.CalculateNextMoveNr() - 1;

            Console.WriteLine("");
            Console.WriteLine($"-------------- Game stats --------------");
            Console.WriteLine($"- Piatto: {_piatto} euro (last move #{lastMoveNr})");
            Console.WriteLine("");
            foreach (var player in _players)
            {
                Console.WriteLine($"- {player.Key}: {player.Value.Balance} euro");
            }
            Console.WriteLine($"----------------------------------------");
            Console.WriteLine("");            
        }

        private void MakeTurn()
        {
            //CheckIfILost()
            CheckForMinPlayers();
            CheckPreviousMove();

            if (_piatto <= 0)
                AskInitialBet();

            PrintGameState();
            var bet = MakeBet();
            var card = UnveilCard();
            var total = Evaluate(card);

            var moveNr = ServicePool.DbService.MoveDb.CalculateNextMoveNr();

            var move = new Move { 
                Author = _myName,
                Bet = bet,
                HasWin = card.Valore > 5,
                Number = moveNr,
                TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Total = total
            };
            BroadcastMove(move);
            // CheckIfILost()
        }

        public void MakeTurnAndPass()
        {
            MakeTurn();
            PassTurn(false);
        }

        public void MakeInitialBet(double amount)
        {
            _players[_myName].MakeBet(amount);
            _players[_myName].Lose(false);
            _piatto += amount;

            var moveNr = ServicePool.DbService.MoveDb.CalculateNextMoveNr();

            var move = new Move()
            {
                Author = _myName,
                Bet = amount,
                HasWin = false,
                Number = moveNr,
                TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Total = amount
            };
            BroadcastMove(move);
        }

        private double Evaluate(Card card)
        {
            if (card.Valore <= 5)
                return EvaluateLose(card);
            else
                return EvaluateWin(card);
        }

        private double EvaluateWin(Card card)
        {
            double total;
            var kingUnveiled = (card.Valore == 10);
            var amountBet = (kingUnveiled) ? _players[_myName].Bet * 2 : _players[_myName].Bet;
            if (amountBet > _piatto)
            {
                 total = _piatto;
                _players[_myName].Win(_piatto);
                Console.WriteLine($"You win a Piatto of {_piatto} euro");
                _piatto = 0;
            }
            else
            {
                total = amountBet;
                _players[_myName].Win(amountBet);
                _piatto -= amountBet;
                Console.WriteLine($"You win {amountBet} euro from the Piatto");
            }
            return total;
        }

        private double EvaluateLose(Card card)
        {
            var aceUnveiled = (card.Valore == 1);
            var amountBet = (aceUnveiled) ? _players[_myName].Bet * 2 : _players[_myName].Bet;
            var total = (amountBet > _players[_myName].Balance) ? _players[_myName].Balance : amountBet;
            if (amountBet > _players[_myName].Balance)
            {
                _piatto += _players[_myName].Balance;
                Console.WriteLine($"You lose -> balance: 0 euro");
            }
            else
            {
                _piatto += amountBet;
                Console.WriteLine($"You lose {amountBet} euro");
            }
            _players[_myName].Lose(aceUnveiled);
            return total;
        }


        private Card UnveilCard()
        {
            Card unveiledCard = _deck.Draw();
            Console.WriteLine($"Unveiled card: {unveiledCard.Valore} di {unveiledCard.Seme}");
            return unveiledCard;
        }

        private double MakeBet()
        {
            Console.WriteLine($"Now it's your turn. Balance: {_players[_myName].Balance} euro");
            Console.WriteLine($"Current piatto value: {_piatto}. Minimum bet: {Constants.MIN_BET} euro");
            double bet;

            while (true)
            {
                Console.Write("Insert your bet value: ");

                try
                {
                    bet = double.Parse(s: Console.ReadLine());
                    _players[_myName].MakeBet(bet);
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return bet;
        }

        private void CheckPreviousMove()
        {
            
        }

        private void AskInitialBet()
        {
            try
            {
                MakeInitialBet(Constants.MIN_BET);
                Console.WriteLine("-- Initial bet just made");
                ServicePool.P2PService.AskInitialBet(Constants.MIN_BET);
            }
            catch (GameException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ChangeDealer()
        {
            TurnNr = 0;
            IsDealer = false;
            PassTurn(true);
        }

        public void PassTurn(bool nextDealer)
        {
            try
            {
                ServicePool.P2PService.PassTurn(nextDealer);
            }
            catch (GameException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ShuffleDeck()
        {
            _deck.Shuffle();
            ServicePool.P2PService.SendSynDeckRequest(_deck);
        }

        private void CheckForMinPlayers()
        {
            if(_players.Count < 2)
                throw new GameException("Not enough players to play a turn");
        }

        private static void BroadcastMove(Move mv)
        {
            // broadcast transaction to all peer not including myself.
            ServicePool.P2PService.BroadcastMove(mv);
        }

        public void UpdateState(Move mv)
        {
            Console.WriteLine($"Updating game state according to move nr.{mv.Number} made by {mv.Author}");
            var oldPiatto = _piatto;
            if (mv.HasWin)
            {
                _piatto -= mv.Total;
                _players[mv.Author].Win(mv.Total);
            }
            else
            {
                _piatto += mv.Total;
                _players[mv.Author].MakeBet(mv.Total);
                _players[mv.Author].Lose(false);
            }
            _deck.Draw();
            // Print game state or at least the new updated value.
            Console.WriteLine($"Piatto was: {oldPiatto} euro now is: {_piatto} euro");
        }

        private void EndGame(bool win)
        {
            Console.WriteLine("****** GAME FINISHED ******");

            if(!win) Console.WriteLine("*** You lost!");
            else {
                var lastPlayerBalance = _players[_myName].Balance;
                Console.WriteLine("*** Last player remained with {0}", lastPlayerBalance);
                Console.WriteLine("*** You win!");
            }
        }


    }
}
