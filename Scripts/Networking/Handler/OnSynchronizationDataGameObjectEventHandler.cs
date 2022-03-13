namespace EUN.Networking
{
    using EUN.Common;
    using EUN.Constant;

    internal class OnSynchronizationDataGameObjectEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnSynchronizationDataGameObject;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
#if EUN
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var ezyArray = parameters.GetEzyArray(ParameterCode.Data);

            var objectId = ezyArray.get<int>(0);
            var synchronizationData = ezyArray.get<object>(1);

            if (peer.room.GameObjectDic.ContainsKey(objectId))
            {
                peer.room.GameObjectDic[objectId].SynchronizationData = synchronizationData;
            }

            if (peer.ezyViewDic.ContainsKey(objectId))
            {
                var view = peer.ezyViewDic[objectId];
                if (view)
                {
                    foreach (var behaviour in view.ezyBehaviourLst)
                    {
                        if (behaviour) behaviour.OnEzySynchronization(synchronizationData);
                    }
                }
            }
#endif
        }
    }
}