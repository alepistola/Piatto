using CartaAlta.Game;
using CartaAlta.Grpc;
using Google.Protobuf.Collections;
using System.Collections.Generic;

namespace CartaAlta.Utils
{
    public static class Extensions
    {
        public static string StringJoin(this IEnumerable<string> values, string separator)
        {
            return string.Join(separator, values);
        }

        public static string MoveToString(Move mv)
        {
            return mv.MoveType switch
            {
                MoveType.InitialBet => PrintInitialBetMove(mv),
                MoveType.GameBet => PrintGameBetMove(mv),
                _ => String.Empty,
            };
        }

        private static string PrintInitialBetMove(Move mv)
        {
            return String.Format("[{0}] Move #{1}: player {2} has made initial bet of {3} euro", mv.TimeStamp, mv.Number, mv.Author, mv.Total);
        }

        private static string PrintGameBetMove(Move mv)
        {
            if (mv.HasWin)
                return String.Format("[{0}] Move #{1}: player {2} has bet {3} euro and has won {4} euro", mv.TimeStamp, mv.Number, mv.Author, mv.Bet, mv.Total);
            else
                return String.Format("[{0}] Move #{1}: player {2} has bet {3} euro and has lost {4} euro", mv.TimeStamp, mv.Number, mv.Author, mv.Bet, mv.Total);
        }
    }
}