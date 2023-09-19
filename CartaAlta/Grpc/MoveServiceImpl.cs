using CartaAlta.Services;
using Grpc.Core;

namespace CartaAlta.Grpc
{
    public class MoveServiceImpl : MoveService.MoveServiceBase
    {
        public override Task<MoveStatus> BroadcastMove(MovePost req, ServerCallContext context)
        {
            Console.WriteLine("-- Received {0}, From: {1}", Utils.Extensions.MoveToString(req.Move), req.SendingFrom);

            try
            {
                ServicePool.DbService.MoveDb.Add(req.Move);
                UpdateStatesWrt(req.Move);

                return Task.FromResult(new MoveStatus
                {
                    Status = true,
                    Message = "Move received successfully!"
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new MoveStatus
                {
                    Status = false,
                    Message = $"Error while processing move nr. {req.Move.Number} ({ex.Message})!"
                });

            }            
        }

        private static void UpdateStatesWrt(Move mv)
        {
            try
            {
                ServicePool.GameEngine.UpdateState(mv);
                ServicePool.CrashDetectionService.UpdateState();
            }
            catch (Utils.GameException ex)
            {
                Console.WriteLine(ex.ExceptionMessage);
                ServicePool.Stop();
                Environment.Exit(0);
            }
            
        }
    }
}
