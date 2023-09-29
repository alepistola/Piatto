namespace Piatto.Game
{
    public class Player
    {
        public string Name { get; }
        public double Balance { get; private set; }
        public double MinBet { get; }
        public double Bet { get; private set; }

        public Player(string name, double balance, double minBet)
        {
            Name = name;
            Balance = balance;
            MinBet = minBet;
            Bet = 0;
        }

        public void MakeBet(double bet)
        {
            if (bet < MinBet || bet > Balance)
            {
                throw new ArgumentException("Invalid bet amount.");
            }

            Bet = bet;
        }

        public void Win(double winPot)
        {
            Balance += winPot;
            Bet = 0;
        }

        public void Lose(bool aceUnveiled)
        {
            if (aceUnveiled)
                Balance -= (Bet * 2);
            else
                Balance -= Bet;

            Bet = 0;
        }

        public bool HaveLost()
        {
            return Balance <= MinBet;
        }
    }
}
