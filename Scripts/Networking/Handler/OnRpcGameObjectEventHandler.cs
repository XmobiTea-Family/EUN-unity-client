namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

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
                    foreach (var behaviour in view.eunBehaviourLst)
                    {
                        if (behaviour) behaviour.EUNRPC(eunRPCCommand, rpcData);
                    }
                }
            }
        }
    }
}