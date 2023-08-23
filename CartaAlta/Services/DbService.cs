using CartaAlta.Db;
using LiteDB;

namespace CartaAlta.Services
{
    public class DbService
    {
        private readonly LiteDatabase DB_MOVE;
        // private readonly LiteDatabase DB_MOVE_HISTORY;
        private readonly LiteDatabase DB_PEER;

        public MoveDb MoveDb { get; set; }
        public PeerDb PeerDb { get; set; }

        // I use multiple database, to minimize database size for transaction, block
        // size will smaller for each database
        public DbService()
        {
            //create db folder
            if (!System.IO.Directory.Exists(@"DbFiles"))
                System.IO.Directory.CreateDirectory(@"DbFiles");

            DB_MOVE = InitializeDatabase(@"DbFiles//move.db");
            DB_PEER = InitializeDatabase(@"DbFiles//peer.db");
        }

        private static LiteDatabase InitializeDatabase(string path)
        {
            return new LiteDatabase(path);
        }

        public void Start()
        {
            Console.WriteLine("... DB Service is starting");
            MoveDb = new MoveDb(DB_MOVE);
            PeerDb = new PeerDb(DB_PEER);
            Console.WriteLine("...... DB Service is ready");
        }

        public void Stop()
        {
            Console.WriteLine("... DB Service is stopping...");
            DB_PEER.Dispose();
            DB_MOVE.Dispose();
            Console.WriteLine("... DB Service has been disposed");
        }
    }
}