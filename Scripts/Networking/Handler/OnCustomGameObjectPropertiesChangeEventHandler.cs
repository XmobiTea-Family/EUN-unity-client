namespace EUN.Networking
{
#if EUN
    using com.tvd12.ezyfoxserver.client.entity;
#endif
    using EUN.Common;
    using EUN.Constant;

    internal class OnCustomGameObjectPropertiesChangeEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnCustomGameObjectPropertiesChange;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
#if EUN
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var ezyArray = parameters.GetEzyArray(ParameterCode.Data);
            var objectId = ezyArray.get<int>(0);

            if (!peer.room.GameObjectDic.ContainsKey(objectId)) return;

            var roomGameObject = peer.room.GameObjectDic[objectId];

            var customGameObjectProperties = new CustomHashtable(ezyArray.get<EzyObject>(1));
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
#endif
        }
    }
}