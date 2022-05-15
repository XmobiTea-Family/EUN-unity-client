namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    internal class OnTransferOwnerGameObjectEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnTransferOwnerGameObject;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var eunArray = parameters.GetEUNArray(ParameterCode.Data);

            var objectId = eunArray.GetInt(0);
            var newOwnerId = eunArray.GetInt(1);

            if (peer.room.GameObjectDic.ContainsKey(objectId))
            {
                var roomGameObject = peer.room.GameObjectDic[objectId];

                roomGameObject.OwnerId = newOwnerId;

                var newOwner = peer.room.RoomPlayerLst.Find(x => x.PlayerId == newOwnerId);

                foreach (var view in peer.eunViewLst)
                {
                    if (view)
                    {
                        if (objectId == view.RoomGameObject.ObjectId)
                        {
                            foreach (var behaviour in view.eunBehaviourLst)
                            {
                                if (behaviour != null) behaviour.OnEUNTransferOwnerGameObject(newOwner);
                            }
                        }
                    }
                }

                var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.OnEUNTransferOwnerGameObject(roomGameObject, newOwner);
                }
            }
        }
    }
}
