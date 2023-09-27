using CartaAlta.Services;
using Grpc.Net.Client;
using CartaAlta.Grpc;
using CartaAlta.Game;
using CartaAlta.Utils;
using Grpc.Core;

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
        private bool _endGameRequestSent = false;

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
                    catch(RpcException)
                    {
                        Console.WriteLine($"--- Fail: impossible to reach {peer.Address}, it may have crashed");
                        HandlePeerCrash(peer);
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

        private void HandlePeerCrash(Peer peer)
        {
            if (ServicePool.DbService.PeerDb.RemoveByAddress(peer.Address) == 1)
            {
                ServicePool.P2PService.UpdateAdjacentPeer();
                ServicePool.GameEngine.RemovePlayerByName(peer.Name);
                Console.WriteLine($"--- Removed peer {peer.Address} ({peer.Name}), new adjacent peer {ServicePool.P2PService.GetAdjacentNodeAddress()}");
                SignalPeerCrash(peer);
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
                    catch (RpcException)
                    {
                        Console.WriteLine($"--- Fail: impossible to reach {peer.Address}, it may have crashed");
                        HandlePeerCrash(peer);
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
                if (_adjacentNode.Address == _nodeAddress) throw new GameException("No adjacent node found!", true);
                Console.WriteLine("-- Passing turn to {0}", _adjacentNode.Address);
                GrpcChannel channel = GrpcChannel.ForAddress(_adjacentNode.Address);
                var gameService = new GameService.GameServiceClient(channel);
                var response = gameService.PassTurn(new PassTurnRequest{ Dealer = nextDealer });
                if (response.Status == true)
                    Console.WriteLine("--- Done: correctly passed!");
                else
                    Console.WriteLine("--- Pass turn request sent but ack not received from {0} (Err: {1})", _adjacentNode.Address, response.Message);
            }
            catch(GameException)
            {
                Console.WriteLine($"--- Fail: an error occured while trying to contact adjacent node");
            }
            catch (RpcException)
            {
                Console.WriteLine($"--- Fail: impossible to reach {_adjacentNode.Address}, it may have crashed");
                HandlePeerCrash(_adjacentNode);
                PassTurn(nextDealer);                
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
                    catch (RpcException)
                    {
                        Console.WriteLine($"--- Fail: impossible to reach {peer.Address}, it may have crashed");
                        HandlePeerCrash(peer);
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
            _endGameRequestSent = true;
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

        private void SignalPeerCrash(Peer crashed)
        {
            var knownPeers = ServicePool.DbService.PeerDb.GetAllToList();

            foreach (var peer in knownPeers)
            {
                if (!_nodeAddress.Equals(peer.Address))
                {
                    Console.WriteLine("-- Signaling peer crash ({1}) to {0}", peer.Address, crashed.Address);
                    GrpcChannel channel = GrpcChannel.ForAddress(peer.Address);
                    var gameService = new GameService.GameServiceClient(channel);
                    try
                    {
                        var response = gameService.SignalCrash(new CrashInfo { NodeAddress = crashed.Address, PlayerName = crashed.Name });
                        if (response.Status == true)
                            Console.WriteLine("--- Done: {0} has just removed {1}", peer.Address, crashed.Address);
                        else
                        {
                            Console.WriteLine("--- Remove peer ack received with error from {0}:", peer.Address);
                            Console.WriteLine($"--- {response.Message}");
                        }
                    }
                    catch (RpcException)
                    {
                        Console.WriteLine($"--- Fail: impossible to reach {peer.Address}, it may have crashed too");
                        HandlePeerCrash(peer);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(".. Failed to signal peer ({0}) to {1}", crashed.Address, peer.Address);
                        // throw new GameException(".. Failed to send syn deck request to {0}", peer.Address);
                    }
                }
            }
        }

        public void Stop()
        {
            if(!(_endGameRequestSent))
                NotifyEndGame();

            if (ServicePool.GameEngine.IsMakingTurn)
                PassTurn(ServicePool.GameEngine.IsDealer);
        }

        public bool Ping(Peer peer)
        {
            GrpcChannel channel = GrpcChannel.ForAddress(peer.Address);
            var gameService = new GameService.GameServiceClient(channel);
            try
            {
                var response = gameService.Ping(new GameServiceRequest{ Message = "Ping!"});
                if (response.Status == true) return true;
                else return false;
            }
            catch (RpcException)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}