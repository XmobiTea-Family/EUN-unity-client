namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client are not inroom and send JoinRoom(), CreateRoom(), JoinRandomRoom() or JoinOrCreateRoom() success
    /// </summary>
    internal class OnJoinRoomEventHandler : IServerEventHandler
    {
        public int getEventCode()
        {
            return EventCode.OnJoinRoom;
        }

        public void handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            var parameters = operationEvent.getParameters();
            var room = new Room(parameters.getEUNArray(ParameterCode.Data));
            peer.room = room;

            var roomPlayer = room.roomPlayerLst.Find(x => x.userId.Equals(EUNNetwork.userId));
            peer.playerId = roomPlayer == null ? -1 : roomPlayer.playerId;

            var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.onEUNJoinRoom();
            }

            var roomGameObjectNeedCreateLst = peer.getListGameObjectNeedCreate();
            if (roomGameObjectNeedCreateLst != null && roomGameObjectNeedCreateLst.Count != 0)
            {
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null)
                    {
                        foreach (var roomGameObject in roomGameObjectNeedCreateLst)
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

    }

}
