namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and transfer room game object success
    /// </summary>
    internal class OnTransferOwnerGameObjectEventHandler : IServerEventHandler
    {
        public int getEventCode()
        {
            return EventCode.OnTransferOwnerGameObject;
        }

        public void handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.getParameters();
            var eunArray = parameters.getEUNArray(ParameterCode.Data);

            var objectId = eunArray.getInt(0);
            var newOwnerId = eunArray.getInt(1);

            if (peer.room.gameObjectDict.ContainsKey(objectId))
            {
                var roomGameObject = peer.room.gameObjectDict[objectId];

                roomGameObject.ownerId = newOwnerId;

                var newOwner = peer.room.roomPlayerLst.Find(x => x.playerId == newOwnerId);

                foreach (var view in peer.eunViewLst)
                {
                    if (view)
                    {
                        if (objectId == view.roomGameObject.objectId)
                        {
                            foreach (var behaviour in view.eunBehaviourLst)
                            {
                                if (behaviour != null) behaviour.onEUNTransferOwnerGameObject(newOwner);
                            }
                        }
                    }
                }

                var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.onEUNTransferOwnerGameObject(roomGameObject, newOwner);
                }
            }
        }

    }

}
