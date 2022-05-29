namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and someone inroom send DestroyGameObjectRoom() request
    /// </summary>
    internal class OnDestroyGameObjectEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnDestroyGameObject;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var eunArray = parameters.GetEUNArray(ParameterCode.Data);
            var objectId = eunArray.GetInt(0);

            RoomGameObject roomGameObject = null;

            if (peer.room.GameObjectDic.ContainsKey(objectId))
            {
                roomGameObject = peer.room.GameObjectDic[objectId];
                peer.room.GameObjectDic.Remove(objectId);
            }

            if (roomGameObject != null)
            {
                foreach (var view in peer.eunViewLst)
                {
                    if (view)
                    {
                        if (view.RoomGameObject.ObjectId == objectId)
                        {
                            foreach (var behaviour in view.eunBehaviourLst)
                            {
                                if (behaviour != null) behaviour.OnEUNDestroyGameObjectRoom();
                            }

                            break;
                        }
                    }
                }

                var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.OnEUNDestroyGameObjectRoom(roomGameObject);
                }
            }
        }
    }
}
