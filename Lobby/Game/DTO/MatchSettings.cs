namespace Lobby.Game.DTO
{
    public class MatchSettings
    {
        public MatchSettings() { GameNr = (new Random().Next(1, 20)); }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int StartingBalance { get; set; }
        public int MinBet { get; set; }
        public int GameNr { get; set; }
    }
}
