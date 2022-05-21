namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    internal class OnSynchronizationDataGameObjectEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnSynchronizationDataGameObject;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var eunArray = parameters.GetEUNArray(ParameterCode.Data);

            var objectId = eunArray.GetInt(0);
            var synchronizationData = eunArray.GetObject(1);

            if (peer.room.GameObjectDic.ContainsKey(objectId))
            {
                peer.room.GameObjectDic[objectId].SynchronizationData = synchronizationData;
            }

            if (peer.eunViewDic.ContainsKey(objectId))
            {
                var view = peer.eunViewDic[objectId];
                if (view)
                {
                    foreach (var behaviour in view.eunBehaviourLst)
                    {
                        if (behaviour != null) behaviour.OnEUNSynchronization(synchronizationData);
                    }
                }
            }
        }
    }
}
