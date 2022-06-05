namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and room game object send EUNRPC() success
    /// </summary>
    internal class OnRpcGameObjectEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnRpcGameObject;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var eunArray = parameters.GetEUNArray(ParameterCode.Data);

            var objectId = eunArray.GetInt(0);
            var eunRPCCommand = eunArray.GetInt(1);
            var rpcData = eunArray.GetEUNArray(2);

            if (peer.eunViewDic.ContainsKey(objectId))
            {
                var view = peer.eunViewDic[objectId];
                if (view)
                {
                    var eunBehaviourLst = view._eunBehaviourLst;
                    for (var i = 0; i < eunBehaviourLst.Count; i++)
                    {
                        var behaviour = eunBehaviourLst[i];
                        if (behaviour != null) behaviour.EUNRPC(eunRPCCommand, rpcData);
                    }

                    var eunManagerBehaviourLst = view._eunManagerBehaviourLst;
                    for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                    {
                        var behaviour = eunManagerBehaviourLst[i];
                        if (behaviour != null) behaviour.EUNRPC(eunRPCCommand, rpcData);
                    }
                }
            }
        }
    }
}
