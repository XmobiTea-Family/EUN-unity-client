namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class RpcGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.RpcGameObjectRoom;

        protected override bool Reliable => false;

        public RpcGameObjectRoomOperationRequest(EUNTargets targets, int objectId, int eunRPCCommand, object rpcData, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Add(ParameterCode.EunRPCCommand, eunRPCCommand)
                .Add(ParameterCode.RpcData, rpcData)
                .Build();

            if (targets != EUNTargets.All) Parameters.Add(ParameterCode.EUNTargets, (int)targets);

        }
    }
}