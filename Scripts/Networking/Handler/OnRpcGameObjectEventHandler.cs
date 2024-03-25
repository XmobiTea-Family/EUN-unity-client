namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and room game object send EUNRPC() success
    /// </summary>
    internal class OnRpcGameObjectEventHandler : IServerEventHandler
    {
        public int getEventCode()
        {
            return EventCode.OnRpcGameObject;
        }

        public void handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.getParameters();
            var eunArray = parameters.getEUNArray(ParameterCode.Data);

            var objectId = eunArray.getInt(0);
            var eunRPCCommand = eunArray.getInt(1);
            var rpcData = eunArray.getEUNArray(2);

            if (peer.eunViewDict.ContainsKey(objectId))
            {
                var view = peer.eunViewDict[objectId];
                if (view)
                {
                    foreach (var behaviour in view.eunBehaviourLst)
                    {
                        if (behaviour != null) behaviour.eunRpc(eunRPCCommand, rpcData);
                    }
                }
            }
        }

    }

}
