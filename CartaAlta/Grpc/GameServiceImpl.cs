using CartaAlta.Services;
using Grpc.Core;

namespace CartaAlta.Grpc
{
    public class GameServiceImpl : GameService.GameServiceBase
    {
        public override Task<GameServiceResponse> SyncDeck(DeckState req, ServerCallContext context)
        {
            try
            {
                Console.WriteLine("-- Received syn deck state from: {0}", req.DealerName);
                ServicePool.GameEngine.SetSynDeck(req.Cards.ToList());

                return Task.FromResult(new GameServiceResponse
                {
                    Status = true,
                    Message = "Syn deck completed!"
                });

            } 
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return Task.FromResult(new GameServiceResponse
                {
                    Status = false,
                    Message = "Error occured while syn deck"
                });
            }
        }

        public override Task<GameServiceResponse> PassTurn(PassTurnRequest request, ServerCallContext context)
        {
            Console.WriteLine("-- Received pass turn request");
            try
            {
                if(ServicePool.GameEngine.IsDealer)
                    Task.Run(() => ServicePool.GameEngine.DealerTurn());
                else
                {
                    if(request.Dealer == true)
                    {
                        ServicePool.GameEngine.IsDealer = true;
                        Task.Run(() => ServicePool.GameEngine.DealerTurn());
                    }
                    else
                        Task.Run(() => ServicePool.GameEngine.MakeTurnAndPass());
                }

                return Task.FromResult(new GameServiceResponse
                {
                    Status = true,
                    Message = String.Format("Correctly received turn request")
                });

            }
            catch (Exception e)
            {
                return Task.FromResult(new GameServiceResponse
                {
                    Status = false,
                    Message = String.Format($"Error occured while processing pass turn request {e.Message}")
                });
            }
        }

        public override Task<GameServiceResponse> AskInitialBet(InitialBetRequest request, ServerCallContext context)
        {
            try
            {
                Console.WriteLine("-- Received initial bet request of {0} euro", request.Amount);
                // va fatto il broadcast per aggiornare il balance di tutti
                ServicePool.GameEngine.MakeInitialBet(request.Amount);

                Console.WriteLine("-- Initial bet just made");

                return Task.FromResult(new GameServiceResponse
                {
                    Status = true,
                    Message = String.Format("Initial bet just made")
                });

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Task.FromResult(new GameServiceResponse
                {
                    Status = false,
                    Message = String.Format("Error occured while trying to make the initial bet")
                });
            }
        }

        public override Task<GameServiceResponse> EndGame(EndGameRequest request, ServerCallContext context)
        {
            try
            {
                Console.WriteLine("-- Received terminate game request from {0}", request.ToRemove);

                if (ServicePool.DbService.PeerDb.RemoveByAddress(request.ToRemove) == 1)
                {
                    ServicePool.P2PService.UpdateAdjacentPeer();
                    Console.WriteLine($"-- Removed peer {request.ToRemove}, new adjacent peer {ServicePool.P2PService.GetAdjacentNodeAddress()}");

                    return Task.FromResult(new GameServiceResponse
                    {
                        Status = true,
                        Message = String.Format("Initial bet just made")
                    });
                }
                else
                {
                    return Task.FromResult(new GameServiceResponse
                    {
                        Status = false,
                        Message = String.Format($"Peer {request.ToRemove} not found!")
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Task.FromResult(new GameServiceResponse
                {
                    Status = false,
                    Message = String.Format($"Error occured while trying to remove {request.ToRemove}")
                });
            }
        }
    }
}
