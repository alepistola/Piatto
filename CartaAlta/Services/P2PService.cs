using CartaAlta.Services;
using Grpc.Net.Client;
using CartaAlta.Grpc;
using CartaAlta.Game;
using CartaAlta.Utils;

namespace CartaAlta.P2P
{
    /// <summary>
    /// This class for communicating with other peer, such as to broadcasting moves,
    /// synchronizing deck.
    /// </summary>
    public class P2PService
    {
        private readonly string _nodeAddress;
        private Peer? _adjacentNode;
        private readonly string _nodeName;

        public P2PService()
        {
            _nodeAddress = DotNetEnv.Env.GetString("NODE_ADDRESS");
            _nodeName = DotNetEnv.Env.GetString("NODE_NAME");
            Console.WriteLine("... P2P service is starting");
            _adjacentNode = FindAdjacentPeer();
            Console.WriteLine("... P2P service: node {0} -> adjacent node = {1}", _nodeAddress, _adjacentNode);
            Console.WriteLine("...... P2P service is ready");
        }

        private Peer? FindAdjacentPeer()
        {
            var peers = ServicePool.DbService.PeerDb.GetAllToList();
            return (peers.SkipWhile(x => x.Address != _nodeAddress).Skip(1).DefaultIfEmpty(peers[0]).FirstOrDefault());
        }

        // In theory it returns _nodeAddress if it is empty
        public void UpdateAdjacentPeer()
        {
            var peers = ServicePool.DbService.PeerDb.GetAllToList();
            _adjacentNode = (peers.SkipWhile(x => x.Address != _nodeAddress).Skip(1).DefaultIfEmpty(peers[0]).FirstOrDefault());
        }

        public string GetAdjacentNodeAddress()
        {
            return _adjacentNode?.Address ?? _nodeAddress;
        }


        public void SendSynDeckRequests(Deck deck)
        {
            var knownPeers = ServicePool.DbService.PeerDb.GetAllToList();

            foreach (var peer in knownPeers)
            {
                if (!_nodeAddress.Equals(peer.Address))
                {
                    Console.WriteLine("-- Sending syn deck request to {0}", peer.Address);
                    GrpcChannel channel = GrpcChannel.ForAddress(peer.Address);
                    var gameService = new GameService.GameServiceClient(channel);
                    try
                    {
                        var currentDeckState = new DeckState();
                        currentDeckState.Cards.AddRange(deck.GetCards());
                        currentDeckState.DealerName = _nodeName;

                        var response = gameService.SyncDeck(currentDeckState);
                        if (response.Status == true)
                            Console.WriteLine("--- Done: received syn deck ack from {0}", peer.Address);
                        else
                            Console.WriteLine("--- Syn deck request sent but ack not received from {0}", peer.Address);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(".. Failed to send syn deck request to {0}", peer.Address);
                        // throw new GameException(".. Failed to send syn deck request to {0}", peer.Address);
                    }
                }
            }
        }

        public void BroadcastMove(Move mv)
        {
            ServicePool.DbService.MoveDb.Add(mv);
            var knownPeers = ServicePool.DbService.PeerDb.GetAllToList();

            foreach (var peer in knownPeers)
            {
                if (!_nodeAddress.Equals(peer.Address))
                {
                    Console.WriteLine("-- Broadcasting move to {0}", peer.Address);
                    GrpcChannel channel = GrpcChannel.ForAddress(peer.Address);
                    var moveService = new MoveService.MoveServiceClient(channel);
                    try
                    {
                        var response = moveService.BroadcastMove(new MovePost
                        {
                            Move = mv,
                            SendingFrom = _nodeAddress
                        });

                        if (response.Status == true)
                            Console.WriteLine("--- Done: received move ack from {0}", peer.Address);
                        else
                            Console.WriteLine("--- Syn move request sent but ack not received from {0}", peer.Address);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(".. Failed to send broadcast request to {0}", peer.Address);
                        // throw new GameException(".. Failed to send syn deck request to {0}", peer.Address);
                    }
                }
            }
        }

        public void PassTurn(bool nextDealer)
        {
            try
            {
                if (_adjacentNode is null) throw new GameException("No adjacent node found!");
                Console.WriteLine("-- Passing turn to {0}", _adjacentNode.Address);
                GrpcChannel channel = GrpcChannel.ForAddress(_adjacentNode.Address);
                var gameService = new GameService.GameServiceClient(channel);
                var response = gameService.PassTurn(new PassTurnRequest{ Dealer = nextDealer });
                if (response.Status == true)
                    Console.WriteLine("--- Done: correctly passed!");
                else
                    Console.WriteLine("--- Pass turn request sent but ack not received from {0} (Err: {1})", _adjacentNode.Address, response.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(".. Failed to send pass turn request ");
                throw;
            }
        }

        public void AskInitialBet(double amount)
        {
            var knownPeers = ServicePool.DbService.PeerDb.GetAllToList();

            foreach (var peer in knownPeers)
            {
                if (!_nodeAddress.Equals(peer.Address))
                {
                    Console.WriteLine("-- Sending initial bet request to {0}", peer.Address);
                    GrpcChannel channel = GrpcChannel.ForAddress(peer.Address);
                    var gameService = new GameService.GameServiceClient(channel);
                    try
                    {
                        var response = gameService.AskInitialBet(new InitialBetRequest { Amount = amount });
                        if (response.Status == true)
                            Console.WriteLine("--- Done: received initial bet ack from {0}", peer.Address);
                        else
                            Console.WriteLine("--- Initial bet request sent but ack not received from {0}", peer.Address);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(".. Failed to send initial bet request to {0}", peer.Address);
                        // throw new GameException(".. Failed to send syn deck request to {0}", peer.Address);
                    }
                }
            }
        }

        public void NotifyEndGame()
        {
            var knownPeers = ServicePool.DbService.PeerDb.GetAllToList();

            foreach (var peer in knownPeers)
            {
                if (!_nodeAddress.Equals(peer.Address))
                {
                    Console.WriteLine("-- Sending terminate game request to {0}", peer.Address);
                    GrpcChannel channel = GrpcChannel.ForAddress(peer.Address);
                    var gameService = new GameService.GameServiceClient(channel);
                    try
                    {
                        var response = gameService.EndGame(new EndGameRequest { ToRemove = _nodeAddress});
                        if (response.Status == true)
                            Console.WriteLine("--- Done: received terminate game ack from {0}", peer.Address);
                        else
                        {
                            Console.WriteLine("--- Terminate game request sent ack from {0} received with error:", peer.Address);
                            Console.WriteLine($"--- {response.Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(".. Failed to send terminate game request to {0}", peer.Address);
                        // throw new GameException(".. Failed to send syn deck request to {0}", peer.Address);
                    }
                }
            }
        }
    }
}
/*

        /// <summary>
        /// Do broadcast a transaction to all peer in known peers
        /// </summary>
        /// <param name="tx"></param>
        public void BroadcastMove(Move mv)
        {
            var knownPeers = ServicePool.FacadeService.Peer.GetKnownPeers();
            var nodeAddress = ServicePool.FacadeService.Peer.NodeAddress;
            Parallel.ForEach(knownPeers, peer =>
            {
                if (!nodeAddress.Equals(peer.Address))
                {
                    Console.WriteLine("-- BroadcastTransaction to {0}", peer.Address);
                    GrpcChannel channel = GrpcChannel.ForAddress(peer.Address);
                    var txnService = new TransactionServiceClient(channel);
                    try
                    {
                        var response = txnService.Receive(new TransactionPost
                        {
                            SendingFrom = nodeAddress,
                            Transaction = tx
                        });
                        if (response.Status == Others.Constants.TXN_STATUS_SUCCESS)
                        {
                            Console.WriteLine(".. Done");
                        }
                        else
                        {
                            Console.WriteLine(".. Fail");
                        }
                    }
                    catch
                    {
                        Console.WriteLine(".. Fail");
                    }
                }
            });
        }


        /// <summary>
        /// Sincronizing moves from all peer in known peers
        /// </summary>
        /// <param name="blockService"></param>
        /// <param name="lastBlockHeight"></param>
        /// <param name="peerHeight"></param>
        private void DownloadMoves(BlockServiceClient blockService, long lastBlockHeight, long peerHeight)
        {
            var response = blockService.GetRemains(new StartingParam { Height = lastBlockHeight });
            List<Block> blocks = response.Blocks.ToList();
            blocks.Reverse();

            var lastHeight = 0L;
            foreach (var block in blocks)
            {
                try
                {
                    Console.WriteLine("==== Download block: {0}", block.Height);
                    var status = ServicePool.DbService.BlockDb.Add(block);
                    lastHeight = block.Height;
                    Console.WriteLine("==== Done");
                }
                catch
                {
                    Console.WriteLine("==== Fail");
                }
            }

            if (lastHeight < peerHeight)
            {
                DownloadBlocks(blockService, lastHeight, peerHeight);
            }
        }

        /// <summary>
        /// Checking in db if new peer already in DB
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private bool IsNewPeer(string address)
        {
            var knownPeers = ServicePool.FacadeService.Peer.GetKnownPeers();
            foreach (var peer in knownPeers)
            {
                if (address == peer.Address)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Sincronize blockchain states, make block height same with other peer
        /// </summary>
        public void SyncState()
        {
            var knownPeers = ServicePool.FacadeService.Peer.GetKnownPeers();
            var nodeAddress = ServicePool.FacadeService.Peer.NodeAddress;

            //synchronizing peer
            foreach (var peer in knownPeers)
            {
                if (!nodeAddress.Equals(peer.Address))
                {
                    Console.WriteLine("Sync state to {0}", peer.Address);
                    try
                    {
                        GrpcChannel channel = GrpcChannel.ForAddress(peer.Address);
                        var peerService = new PeerServiceClient(channel);
                        var peerState = peerService.GetNodeState(new NodeParam { NodeIpAddress = nodeAddress });

                        // add peer to db
                        foreach (var newPeer in peerState.KnownPeers)
                        {
                            ServicePool.FacadeService.Peer.Add(newPeer);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            // synchronizing blocks
            knownPeers = ServicePool.FacadeService.Peer.GetKnownPeers();
            foreach (var peer in knownPeers)
            {
                if (!nodeAddress.Equals(peer.Address))
                {
                    try
                    {
                        GrpcChannel channel = GrpcChannel.ForAddress(peer.Address);
                        var peerService = new PeerServiceClient(channel);
                        var peerState = peerService.GetNodeState(new NodeParam { NodeIpAddress = nodeAddress });

                        // local block height
                        var lastBlockHeight = ServicePool.DbService.BlockDb.GetLast().Height;
                        var blockService = new BlockServiceClient(channel);
                        if (lastBlockHeight < peerState.Height)
                        {
                            DownloadBlocks(blockService, lastBlockHeight, peerState.Height);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            Console.WriteLine("---- Sync Done~");
        }
    }
}
*/