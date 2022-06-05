namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and room game object send synchronization data success
    /// </summary>
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

            if (peer.room.gameObjectDic.ContainsKey(objectId))
            {
                peer.room.gameObjectDic[objectId].synchronizationData = synchronizationData;
            }

            if (peer.eunViewDic.ContainsKey(objectId))
            {
                var view = peer.eunViewDic[objectId];
                if (view)
                {
                    var observedComponents = view.observedComponents;

                    for (var j = 0; j < observedComponents.Length; j++)
                    {
                        var behaviour = observedComponents[j];

                        if (behaviour != null)
                        {
                            if (behaviour.gameObject.activeInHierarchy)
                                behaviour.OnEUNSynchronization(synchronizationData);
                        }
                    }
                }
            }
        }
    }
}
