namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and transfer room game object success
    /// </summary>
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

                if (newOwner != null)
                {
                    if (peer.eunViewDic.ContainsKey(objectId))
                    {
                        var view = peer.eunViewDic[objectId];

                        if (view)
                        {
                            var eunBehaviourLst = view.eunBehaviourLst;

                            for (var i = 0; i < eunBehaviourLst.Count; i++)
                            {
                                var behaviour = eunBehaviourLst[i];

                                if (behaviour != null) behaviour.OnEUNTransferOwnerGameObject(newOwner);
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
}
