namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class RpcGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.RpcGameObjectRoom;

        protected override bool Reliable => false;

        /// <summary>
        /// RpcGameObjectRoomOperationRequest
        /// </summary>
        /// <param name="targets">The targets client want send</param>
        /// <param name="objectId">The object id room game object</param>
        /// <param name="eunRPCCommand">The command RPC</param>
        /// <param name="rpcData">The RPC data</param>
        /// <param name="timeout"></param>
        public RpcGameObjectRoomOperationRequest(EUNTargets targets, int objectId, int eunRPCCommand, object rpcData, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Add(ParameterCode.EunRPCCommand, eunRPCCommand)
                .Add(ParameterCode.RpcData, rpcData)
                .Build();

            if (targets != EUNTargets.AllViaServer) Parameters.Add(ParameterCode.EUNTargets, (int)targets);

        }
    }
}
