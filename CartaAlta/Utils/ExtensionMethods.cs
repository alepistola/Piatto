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
            if (mv.HasWin)
                return String.Format("[{0}] Move #{1}: player {2} has bet {3} and has won {4}", mv.TimeStamp, mv.Number, mv.Author, mv.Bet, mv.Total);
            else
                return String.Format("[{0}] Move #{1}: player {2} has bet {3} and has lost {4}", mv.TimeStamp, mv.Number, mv.Author, mv.Bet, mv.Total);
        }
    }
}