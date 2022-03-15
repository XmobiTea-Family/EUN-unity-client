namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    internal class OnRpcGameObjectEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnRpcGameObject;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var customArray = parameters.GetCustomArray(ParameterCode.Data);

            var objectId = customArray.GetInt(0);
            var eunRPCCommand = customArray.GetInt(1);
            var rpcData = customArray.GetCustomArray(2);

            if (peer.ezyViewDic.ContainsKey(objectId))
            {
                var view = peer.ezyViewDic[objectId];
                if (view)
                {
                    foreach (var behaviour in view.ezyBehaviourLst)
                    {
                        if (behaviour) behaviour.EzyRPC(eunRPCCommand, rpcData);
                    }
                }
            }
        }
    }
}