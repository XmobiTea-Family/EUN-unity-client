namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client are not inroom and send JoinRoom(), CreateRoom(), JoinRandomRoom() or JoinOrCreateRoom() success
    /// </summary>
    internal class OnJoinRoomEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnJoinRoom;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            var parameters = operationEvent.GetParameters();
            var room = new Room(parameters.GetEUNArray(ParameterCode.Data));
            peer.room = room;

            var roomPlayer = room.RoomPlayerLst.Find(x => x.UserId.Equals(EUNNetwork.UserId));
            peer.playerId = roomPlayer == null ? -1 : roomPlayer.PlayerId;

            var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.OnEUNJoinRoom();
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
                            var view = behaviour.OnEUNViewNeedCreate(roomGameObject);
                            if (view != null)
                            {
                                view.Init(roomGameObject);
                                peer.eunViewDic[view.RoomGameObject.ObjectId] = view;
                            }
                        }
                    }
                }
            }
        }
    }
}
