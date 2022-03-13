namespace EUN.Networking
{
#if EUN
    using com.tvd12.ezyfoxserver.client.entity;
#endif

    using EUN.Common;
    using EUN.Constant;

    internal class OnRpcGameObjectEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnRpcGameObject;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
#if EUN
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var ezyArray = parameters.GetEzyArray(ParameterCode.Data);

            var objectId = ezyArray.get<int>(0);
            var eunRPCCommand = ezyArray.get<int>(1);
            var rpcData = ezyArray.get<EzyArray>(2);

            //if (peer.room.GameObjectDic.ContainsKey(objectId))
            //{
            //    //peer.room.GameObjectDic[objectId].SynchronizationData = synchronizationData;
            //}

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
#endif
        }
    }
}