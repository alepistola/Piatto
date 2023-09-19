using CartaAlta.Db;
using LiteDB;

namespace CartaAlta.Services
{
    public class DbService
    {
        private readonly LiteDatabase DB_MOVE;
        private readonly LiteDatabase DB_PEER;

        public MoveDb MoveDb { get; set; }
        public PeerDb PeerDb { get; set; }

        // I use multiple database, to minimize database size for transaction, block
        // size will smaller for each database
        public DbService()
        {
            // delete if already exists (for consistent move nr)
            if (Directory.Exists(@"DbFiles"))
                Directory.Delete(@"DbFiles", true);

            // create new
            Directory.CreateDirectory(@"DbFiles");


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
            PeerDb.RemoveAll();
            MoveDb.RemoveAll();
            DB_PEER.Dispose();
            DB_MOVE.Dispose();
            Console.WriteLine("... DB Service has been disposed");
        }
    }
}