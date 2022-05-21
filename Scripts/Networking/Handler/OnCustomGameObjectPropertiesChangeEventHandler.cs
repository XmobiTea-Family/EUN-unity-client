namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

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

            foreach (var view in peer.eunViewLst)
            {
                if (view)
                {
                    if (objectId == view.RoomGameObject.ObjectId)
                    {
                        foreach (var behaviour in view.eunBehaviourLst)
                        {
                            if (behaviour != null) behaviour.OnEUNCustomGameObjectPropertiesChange(customGameObjectProperties);
                        }
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
