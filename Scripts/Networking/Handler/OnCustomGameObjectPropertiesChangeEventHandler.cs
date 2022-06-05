namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and someone inroom send ChangeGameObjectCustomProperties() request
    /// </summary>
    internal class OnCustomGameObjectPropertiesChangeEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnCustomGameObjectPropertiesChange;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var eunArray = parameters.GetEUNArray(ParameterCode.Data);
            var objectId = eunArray.GetInt(0);

            if (!peer.room.GameObjectDic.ContainsKey(objectId)) return;

            var roomGameObject = peer.room.GameObjectDic[objectId];

            var customGameObjectProperties = eunArray.GetEUNHashtable(1);
            var keySet = customGameObjectProperties.Keys();
            foreach (int key in keySet)
            {
                var value = customGameObjectProperties.GetObject(key);

                if (value == null)
                {
                    if (roomGameObject.CustomProperties.ContainsKey(key))
                    {
                        roomGameObject.CustomProperties.Remove(key);
                    }
                }
                else roomGameObject.CustomProperties.Add(key, value);
            }

            if (peer.eunViewDic.ContainsKey(objectId))
            {
                var view = peer.eunViewDic[objectId];

                if (view)
                {
                    var eunBehaviourLst = view.eunBehaviourLst;
                    
                    for (var i = 0; i < eunBehaviourLst.Count; i++)
                    {
                        var behaviour = eunBehaviourLst[i];

                        if (behaviour != null) behaviour.OnEUNCustomGameObjectPropertiesChange(customGameObjectProperties);
                    }
                }
            }

            var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.OnEUNCustomGameObjectPropertiesChange(roomGameObject, customGameObjectProperties);
            }
        }
    }
}
