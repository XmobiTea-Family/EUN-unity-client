namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class RpcGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.RpcGameObjectRoom;

        protected override bool reliable => false;

        /// <summary>
        /// RpcGameObjectRoomOperationRequest
        /// </summary>
        /// <param name="targets">The targets client want send</param>
        /// <param name="objectId">The object id room game object</param>
        /// <param name="eunRPCCommand">The command RPC</param>
        /// <param name="rpcData">The RPC data</param>
        /// <param name="timeout"></param>
        public RpcGameObjectRoomOperationRequest(EUNTargets targets, int objectId, int eunRPCCommand, object rpcData, int timeout = OperationRequest.defaultTimeout) : base(timeout)
        {
            this.parameters = new EUNHashtable.Builder()
                .add(ParameterCode.ObjectId, objectId)
                .add(ParameterCode.EunRPCCommand, eunRPCCommand)
                .add(ParameterCode.RpcData, rpcData)
                .build();

            if (targets != EUNTargets.AllViaServer) parameters.add(ParameterCode.EUNTargets, (int)targets);

        }

    }

}
