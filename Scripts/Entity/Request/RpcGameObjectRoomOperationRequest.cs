namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class RpcGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.RpcGameObjectRoom;

        protected override bool Reliable => false;

        public RpcGameObjectRoomOperationRequest(EzyTargets targets, int objectId, int eunRPCCommand, object rpcData, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Add(ParameterCode.EunRPCCommand, eunRPCCommand)
                .Add(ParameterCode.RpcData, rpcData)
                .Build();

            if (targets != EzyTargets.All) Parameters.Add(ParameterCode.EzyTargets, (int)targets);

        }
    }
}