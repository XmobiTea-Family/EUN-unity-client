namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    internal class OnCustomGameObjectPropertiesChangeEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnCustomGameObjectPropertiesChange;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var customArray = parameters.GetCustomArray(ParameterCode.Data);
            var objectId = customArray.GetInt(0);

            if (!peer.room.GameObjectDic.ContainsKey(objectId)) return;

            var roomGameObject = peer.room.GameObjectDic[objectId];

            var customGameObjectProperties = customArray.GetCustomHashtable(1);
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

            foreach (var view in peer.ezyViewLst)
            {
                if (view)
                {
                    if (objectId == view.RoomGameObject.ObjectId)
                    {
                        foreach (var behaviour in view.ezyBehaviourLst)
                        {
                            if (behaviour) behaviour.OnEzyCustomGameObjectPropertiesChange(customGameObjectProperties);
                        }
                    }
                }
            }

            foreach (var behaviour in peer.ezyManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEzyCustomGameObjectPropertiesChange(roomGameObject, customGameObjectProperties);
            }
        }
    }
}