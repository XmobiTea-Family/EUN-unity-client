namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and someone inroom send DestroyGameObjectRoom() request
    /// </summary>
    internal class OnDestroyGameObjectEventHandler : IServerEventHandler
    {
        public int getEventCode()
        {
            return EventCode.OnDestroyGameObject;
        }

        public void handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.getParameters();
            var eunArray = parameters.getEUNArray(ParameterCode.Data);
            var objectId = eunArray.getInt(0);

            RoomGameObject roomGameObject = null;

            if (peer.room.gameObjectDict.ContainsKey(objectId))
            {
                roomGameObject = peer.room.gameObjectDict[objectId];
                peer.room.gameObjectDict.Remove(objectId);
            }

            if (roomGameObject != null)
            {
                foreach (var view in peer.eunViewLst)
                {
                    if (view)
                    {
                        if (view.roomGameObject.objectId == objectId)
                        {
                            foreach (var behaviour in view.eunBehaviourLst)
                            {
                                if (behaviour != null) behaviour.onEUNDestroyGameObjectRoom();
                            }

                            break;
                        }
                    }
                }

                var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.onEUNDestroyGameObjectRoom(roomGameObject);
                }
            }
        }

    }

}
