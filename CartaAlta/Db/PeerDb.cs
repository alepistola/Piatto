using CartaAlta.Grpc;
using LiteDB;

namespace CartaAlta.Db
{
    /// <summary>
    /// Peer database, for add, update list of peers
    /// </summary>
    public class PeerDb
    {
        private readonly LiteDatabase _db;

        public PeerDb(LiteDatabase db)
        {
            _db = db;
        }

        /// <summary>
        /// Add a peer
        /// </summary>
        public void Add(Peer peer)
        {
            var existingPeer = GetByAddress(peer.Address);
            if (existingPeer is null)
            {
                GetAll().Insert(peer);
            }
        }

        public bool AddBulk(List<Peer> peers)
        {
            try
            {
                var collection = GetAll();

                collection.InsertBulk(peers);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get list of peer, page number and number of row per page
        /// </summary>
        public List<Peer> GetRange(int pageNumber, int resultPerPage)
        {
            var peers = GetAll();

            var query = peers.Query()
                .Offset((pageNumber - 1) * resultPerPage)
                .Limit(resultPerPage).ToList();

            return query;
        }


        /// <summary>
        /// Get all peer
        /// </summary>
        public ILiteCollection<Peer> GetAll()
        {
            var peers = _db.GetCollection<Peer>(CartaAlta.Utils.Constants.TBL_PEERS);
            return peers;
        }

        public List<Peer> GetAllToList()
        {
            return _db.GetCollection<Peer>(CartaAlta.Utils.Constants.TBL_PEERS).FindAll().ToList();
        }

        /// <summary>
        /// Get peer by network address/IP
        /// </summary>
        public Peer GetByAddress(string address)
        {
            var peers = GetAll();
            if (peers is null)
            {
                return null;
            }

            return peers.FindOne(x => x.Address == address);
        }

        public int RemoveByAddress(string address)
        {
            var deletedCount = _db.GetCollection<Peer>(Utils.Constants.TBL_PEERS).DeleteMany(p => p.Address == address);
            return deletedCount;
        }

        public void RemoveAll()
        {
            _db.GetCollection<Peer>(Utils.Constants.TBL_PEERS).DeleteAll();
        }
    }
}