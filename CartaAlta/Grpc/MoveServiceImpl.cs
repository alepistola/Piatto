using CartaAlta.Services;
using Grpc.Core;

namespace CartaAlta.Grpc
{
    public class MoveServiceImpl : MoveService.MoveServiceBase
    {
        public override Task<Move> GetByNumber(RequestNumber requestNumber, ServerCallContext context)
        {
            var move = ServicePool.DbService.MoveDb.GetByNumber(requestNumber.Number);
            return Task.FromResult(move);
        }


        public override Task<MoveStatus> BroadcastMove(MovePost req, ServerCallContext context)
        {
            Console.WriteLine("-- Received {0}, From: {1}", Utils.Extensions.MoveToString(req.Move), req.SendingFrom);

            /*
            var transactionHash = Others.UkcUtils.GetTransactionHash(req.Transaction);
            if (!transactionHash.Equals(req.Transaction.Hash))
            {
                return Task.FromResult(new TransactionStatus
                {
                    Status = Others.Constants.TXN_STATUS_FAIL,
                    Message = "Invalid Transaction Hash"
                });
            }

            var isSignatureValid = VerifySignature(req.Transaction);
            if (!isSignatureValid)
            {
                return Task.FromResult(new TransactionStatus
                {
                    Status = Others.Constants.TXN_STATUS_FAIL,
                    Message = "Invalid Signature"
                });
            }
            */
            //TODO add more validation here
            try
            {
                ServicePool.DbService.MoveDb.Add(req.Move);
                ServicePool.GameEngine.UpdateState(req.Move);

                return Task.FromResult(new MoveStatus
                {
                    Status = true,
                    Message = "Move received successfully!"
                });
            }
            catch (Exception)
            {
                return Task.FromResult(new MoveStatus
                {
                    Status = true,
                    Message = $"Error while processing move nr. {req.Move.Number}!"
                });

            }            
        }
        
    }
}
