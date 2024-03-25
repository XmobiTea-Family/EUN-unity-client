namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and someone inroom send ChangeGameObjectCustomProperties() request
    /// </summary>
    internal class OnGameObjectCustomPropertiesChangeEventHandler : IServerEventHandler
    {
        public int getEventCode()
        {
            return EventCode.OnGameObjectCustomPropertiesChange;
        }

        public void handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.getParameters();
            var eunArray = parameters.getEUNArray(ParameterCode.Data);
            var objectId = eunArray.getInt(0);

            if (!peer.room.gameObjectDict.ContainsKey(objectId)) return;

            var roomGameObject = peer.room.gameObjectDict[objectId];

            var customGameObjectProperties = eunArray.getEUNHashtable(1);
            var keySet = customGameObjectProperties.keys();
            foreach (int key in keySet)
            {
                var value = customGameObjectProperties.getObject(key);

                if (value == null)
                {
                    if (roomGameObject.customProperties.containsKey(key))
                    {
                        roomGameObject.customProperties.remove(key);
                    }
                }
                else roomGameObject.customProperties.add(key, value);
            }

            foreach (var view in peer.eunViewLst)
            {
                if (view)
                {
                    if (objectId == view.roomGameObject.objectId)
                    {
                        foreach (var behaviour in view.eunBehaviourLst)
                        {
                            if (behaviour != null) behaviour.onEUNCustomGameObjectPropertiesChange(customGameObjectProperties);
                        }
                    }
                }
            }

            var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.onEUNCustomGameObjectPropertiesChange(roomGameObject, customGameObjectProperties);
            }
        }

    }

}
