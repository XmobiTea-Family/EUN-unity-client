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
                if (peer.eunViewDic.ContainsKey(objectId))
                {
                    var view = peer.eunViewDic[objectId];

                    if (view)
                    {
                        var eunBehaviourLst = view.eunBehaviourLst;

                        for (var i = 0; i < eunBehaviourLst.Count; i++)
                        {
                            var behaviour = eunBehaviourLst[i];

                            if (behaviour != null) behaviour.OnEUNDestroyGameObjectRoom();
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
