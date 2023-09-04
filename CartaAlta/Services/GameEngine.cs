using CartaAlta.Game;
using CartaAlta.Grpc;
using CartaAlta.Utils;

namespace CartaAlta.Services
{
    public class GameEngine
    {
        public bool IsDealer { get; set; }
        public string DealerName { get; set; }
        private int TurnNr { get; set; }
        private Dictionary<string, Player> _players;
        private Deck _deck;
        private double _piatto;
        private bool _gameFinished;
        private readonly string _myName;


        public GameEngine(List<Peer> peerList, bool isDealer)
        { 
            _players = PlayersFactory.GeneratePlayers(peerList);
            IsDealer = isDealer;
            _deck = new Deck();
            _piatto = 0;
            _myName = DotNetEnv.Env.GetString("NODE_NAME");
            TurnNr = 0;
            _gameFinished = false;
        }

        public void SetSynDeck(List<Card> cards) => _deck = new Deck(cards);

        public void DealerTurn()
        {
            if (!IsDealer)
            {
                Console.WriteLine("You're not the dealer, just wait for your turn..");
                return;
            }
            Thread.Sleep(2000);

            if (!(CheckForMinPlayers()))
                return;

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

            if (!_gameFinished)
                PassTurn(false);
        }


        public bool RemovePlayerByName(string name)
        {
            try
            {
                return _players.Remove(name);
            }
            catch (Exception)
            {
                return false;
            }
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
            if (!(CheckForMinPlayers()))
                return;

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
                Total = total,
                MoveType = MoveType.GameBet,
                DrawnCard = card
            };
            BroadcastMove(move);
            CheckIfILost();

            CheckForMinPlayers();
        }

        private void CheckIfILost()
        {
            if (_players[_myName].HaveLost())
            {
                ServicePool.P2PService.NotifyEndGame();
                PrintEndGame(false); // Send remove notify to others peer? or just leave and let them find out?
            }
                
        }

        public void MakeTurnAndPass()
        {
            MakeTurn();
            if (_gameFinished)
                return;
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
                Number = moveNr,
                TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Total = amount,
                MoveType = MoveType.InitialBet
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
                Console.WriteLine($"You lose all of your euro");
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
            if (_gameFinished) return;
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
                
                if (ex.IsTheOnlyPlayer)
                    throw;

                Console.WriteLine(ex.Message);
            }
        }

        private void ShuffleDeck()
        {
            _deck.Shuffle();
            ServicePool.P2PService.SendSynDeckRequests(_deck);
        }

        private bool CheckForMinPlayers()
        {
            var playingPlayersNr = _players.Where(p => !(p.Value.HaveLost())).Count();
            if(_players.Count < 2 || playingPlayersNr < 2)
            {
                PrintEndGame(true);
                _gameFinished = true;
                return false;
            }
            return true;
        }

        private static void BroadcastMove(Move mv)
        {
            // broadcast transaction to all peer not including myself.
            ServicePool.P2PService.BroadcastMove(mv);
        }

        public void UpdateState(Move mv)
        {
            Console.WriteLine($"Updating game state according to move #{mv.Number} made by {mv.Author}");
            var oldPiatto = _piatto;

            switch (mv.MoveType)
            {
                case MoveType.InitialBet:
                    UpdateStateForInitialBetMove(mv);
                    break;
                case MoveType.GameBet:
                    UpdateStateForGameBetMove(mv);
                    break;
                default:
                    break;
            }
            // Print game state or at least the new updated value.
            Console.WriteLine($"Piatto was: {oldPiatto} euro now is: {_piatto} euro");
        }

        private void UpdateStateForInitialBetMove(Move mv)
        {
            _piatto += mv.Total;
            _players[mv.Author].MakeBet(mv.Total);
            _players[mv.Author].Lose(false);
        }

        private void UpdateStateForGameBetMove(Move mv)
        {
            Console.WriteLine($"Legit check for move #{mv.Number}");

            bool legit = DrawAndCheckLegit(mv.DrawnCard);
            if (!legit)
                throw new GameException($"Card drawn by {mv.Author} not consistent with the one locally unveiled!");

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
        }

        private bool DrawAndCheckLegit(Card remoteDrawn)
        {
            Card localUnveiled = _deck.Draw();
            if (remoteDrawn.Seme == localUnveiled.Seme && remoteDrawn.Valore == localUnveiled.Valore)
            {
                Console.WriteLine("Legit check: ok");
                return true;
            }
            else
            {
                Console.WriteLine("Legit check: error");
                return false;
            }
        }

        private void PrintEndGame(bool win)
        {
            Console.WriteLine("\n****** GAME FINISHED ******");

            if(!win) Console.WriteLine("****** You lost! ******");
            else {
                var lastPlayerBalance = _players[_myName].Balance;
                Console.WriteLine("*** Last player remained with {0}", lastPlayerBalance);
                Console.WriteLine("*** You win!");
            }

            Console.WriteLine("\nPress Ctrl+C to shut down...");
        }


    }
}
