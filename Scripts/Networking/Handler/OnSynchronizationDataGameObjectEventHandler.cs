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
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var customArray = parameters.GetCustomArray(ParameterCode.Data);

            var objectId = customArray.GetInt(0);
            var synchronizationData = customArray.GetObject(1);

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
        }
    }
}