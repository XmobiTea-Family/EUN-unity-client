namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and someone inroom send CreateGameObjectRoom() request
    /// </summary>
    internal class OnCreateGameObjectEventHandler : IServerEventHandler
    {
        public int getEventCode()
        {
            return EventCode.OnCreateGameObject;
        }

        public void handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;
            var parameters = operationEvent.getParameters();

            var roomGameObject = new RoomGameObject(parameters.getEUNArray(ParameterCode.Data));
            peer.room.gameObjectDict[roomGameObject.objectId] = roomGameObject;
            
            var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null)
                {
                    var view = behaviour.onEUNViewNeedCreate(roomGameObject);
                    if (view != null)
                    {
                        view.init(roomGameObject);
                        peer.eunViewDict[view.roomGameObject.objectId] = view;
                    }
                }
            }
        }

    }

}
