using LiteDB;
using CartaAlta.Utils;
using CartaAlta.Grpc;
using CartaAlta.Services;

namespace CartaAlta.Db
{
    /// <summary>
    /// Transaction DB, for add, update transaction
    /// </summary>
    public class MoveDb
    {
        private readonly LiteDatabase _db;

        public MoveDb(LiteDatabase db)
        {
            _db = db;
        }

        /// <summary>
        /// Add some move in smae time
        /// </summary>
        public bool AddBulk(List<Move> moves)
        {
            try
            {
                var collection = GetAll();

                collection.InsertBulk(moves);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Add a move
        /// </summary>
        public bool Add(Move move)
        {
            try
            {
                var moves = GetAll();
                moves.Insert(move);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get All Transactions by Address and with paging
        /// </summary>
        public IEnumerable<Move> GetRangeByAuthor(string author, int pageNumber, int resultsPerPage)
        {
            var moves = GetAll();
            if (moves is null || moves.Count() < 1)
            {
                return null;
            }

            moves.EnsureIndex(x => x.Author);

            var query = moves.Query()
                .OrderByDescending(x => x.TimeStamp)
                .Where(x => x.Author == author)
                .Offset((pageNumber - 1) * resultsPerPage)
                .Limit(resultsPerPage).ToList();

            return query;
        }

        /// <summary>
        /// Get Move by Number
        /// </summary>
        public Move GetByNumber(int number)
        {
            var moves = GetAll();
            if (moves is null || moves.Count() < 1)
            {
                return null;
            }

            moves.EnsureIndex(x => x.Number);

            return moves.FindOne(x => x.Number == number);
        }

        /// <summary>
        /// Get transactions
        /// </summary>
        public IEnumerable<Move> GetRange(int pageNumber, int resultPerPage)
        {
            var transactions = GetAll();
            if (transactions is null || transactions.Count() < 1)
            {
                return null;
            }

            transactions.EnsureIndex(x => x.TimeStamp);

            var query = transactions.Query()
                .OrderByDescending(x => x.TimeStamp)
                .Offset((pageNumber - 1) * resultPerPage)
                .Limit(resultPerPage).ToList();

            return query;
        }

        public IEnumerable<Move> GetLast(int num)
        {
            var moves = GetAll();
            if (moves is null || moves.Count() < 1)
            {
                return null;
            }

            moves.EnsureIndex(x => x.TimeStamp);

            var query = moves.Query()
                .OrderByDescending(x => x.TimeStamp)
                .Limit(num).ToList();

            return query;
        }

        /// <summary>
        /// get one move by address
        /// </summary>
        public Move GetByAuthor(string author)
        {
            var moves = GetAll();
            if (moves is null || moves.Count() < 1)
            {
                return null;
            }

            moves.EnsureIndex(x => x.TimeStamp);
            var move = moves.FindOne(x => x.Author == author);
            return move;
        }

        public int CalculateNextMoveNr()
        {
            return (ServicePool.DbService.MoveDb.GetLast(1) is null) ? 1 : ++(ServicePool.DbService.MoveDb.GetLast(1).ToList().First().Number);
        }

        private ILiteCollection<Move> GetAll()
        {
            return _db.GetCollection<Move>(Constants.TBL_MOVE);
        }
    }
}
