namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    using System.Collections.Generic;

    public class RpcGameObjectRoomToOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.RpcGameObjectRoomTo;

        protected override bool reliable => false;

        /// <summary>
        /// RpcGameObjectRoomToOperationRequest
        /// </summary>
        /// <param name="targetPlayerIds">The player id list in room you want send</param>
        /// <param name="objectId">The object id room game object</param>
        /// <param name="eunRPCCommand">The command RPC</param>
        /// <param name="rpcData">The RPC data</param>
        /// <param name="timeout"></param>
        public RpcGameObjectRoomToOperationRequest(IList<int> targetPlayerIds, int objectId, int eunRPCCommand, object rpcData, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Add(ParameterCode.EunRPCCommand, eunRPCCommand)
                .Add(ParameterCode.RpcData, rpcData)
                .Add(ParameterCode.TargetPlayerIds, targetPlayerIds)
                .Build();

        }
    }
}
