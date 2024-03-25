namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and room game object send synchronization data success
    /// </summary>
    internal class OnSynchronizationDataGameObjectEventHandler : IServerEventHandler
    {
        public int getEventCode()
        {
            return EventCode.OnSynchronizationDataGameObject;
        }

        public void handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.getParameters();
            var eunArray = parameters.getEUNArray(ParameterCode.Data);

            var objectId = eunArray.getInt(0);
            var synchronizationData = eunArray.getObject(1);

            if (peer.room.gameObjectDict.ContainsKey(objectId))
            {
                peer.room.gameObjectDict[objectId].synchronizationData = synchronizationData;
            }

            if (peer.eunViewDict.ContainsKey(objectId))
            {
                var view = peer.eunViewDict[objectId];
                if (view)
                {
                    foreach (var behaviour in view.eunBehaviourLst)
                    {
                        if (behaviour != null) behaviour.onEUNSynchronization(synchronizationData);
                    }
                }
            }
        }

    }

}
