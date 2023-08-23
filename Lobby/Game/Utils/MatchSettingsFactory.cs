using Lobby.Game.DTO;

namespace Lobby.Utils
{
    public static class MatchSettingsFactory
    {
        public static MatchSettings generateFromEnv()
        {
            return new MatchSettings
            {
                MaxPlayers = DotNetEnv.Env.GetInt("MAX_PLAYERS"),
                MinPlayers = DotNetEnv.Env.GetInt("MIN_PLAYERS"),
                MinBet = DotNetEnv.Env.GetInt("MIN_BET"),
                StartingBalance = DotNetEnv.Env.GetInt("STARTING_BALANCE")
            };
        }

    }
}
