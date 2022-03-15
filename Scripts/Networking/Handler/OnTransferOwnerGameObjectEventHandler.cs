namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    internal class OnTransferOwnerGameObjectEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnTransferOwnerGameObject;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var customArray = parameters.GetCustomArray(ParameterCode.Data);

            var objectId = customArray.GetInt(0);
            var newOwnerId = customArray.GetInt(1);

            if (peer.room.GameObjectDic.ContainsKey(objectId))
            {
                var roomGameObject = peer.room.GameObjectDic[objectId];

                roomGameObject.OwnerId = newOwnerId;

                var newOwner = peer.room.RoomPlayerLst.Find(x => x.PlayerId == newOwnerId);

                foreach (var view in peer.ezyViewLst)
                {
                    if (view)
                    {
                        if (objectId == view.RoomGameObject.ObjectId)
                        {
                            foreach (var behaviour in view.ezyBehaviourLst)
                            {
                                if (behaviour) behaviour.OnEzyTransferOwnerGameObject(newOwner);
                            }
                        }
                    }
                }

                foreach (var behaviour in peer.ezyManagerBehaviourLst)
                {
                    if (behaviour) behaviour.OnEzyTransferOwnerGameObject(roomGameObject, newOwner);
                }
            }
        }
    }
}