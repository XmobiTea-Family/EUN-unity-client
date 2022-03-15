namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    using System.Collections.Generic;

    public class RpcGameObjectRoomToOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.RpcGameObjectRoomTo;

        protected override bool Reliable => false;

        public RpcGameObjectRoomToOperationRequest(IList<int> targetPlayerIds, int objectId, int eunRPCCommand, object rpcData, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Add(ParameterCode.EunRPCCommand, eunRPCCommand)
                .Add(ParameterCode.RpcData, rpcData)
                .Add(ParameterCode.TargetPlayerIds, targetPlayerIds)
                .Build();

        }
    }
}