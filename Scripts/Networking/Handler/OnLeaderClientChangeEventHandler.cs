namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and:
    /// Current leader client in current room send ChangeLeaderClient
    /// Current leader client disconnect or LeaveRoom()
    /// 
    /// And handle if client are not inroom and:
    /// CreateRoom() success, JoinRoom() a room does not time to live to destroy room, but it empty room player because they LeaveRoom() or disconnect
    /// </summary>
    internal class OnLeaderClientChangeEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnLeaderClientChange;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var roomPlayer = new RoomPlayer(parameters.GetEUNArray(ParameterCode.Data));

            var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;

            var eunViewLst = peer.eunViewLst;

            var thisRoomPlayer = peer.room.RoomPlayerLst.Find(x => x.UserId.Equals(roomPlayer.UserId));
            if (thisRoomPlayer == null)
            {
                thisRoomPlayer = roomPlayer;
                peer.room.RoomPlayerLst.Add(thisRoomPlayer);

                for (var i = 0; i < eunViewLst.Count; i++)
                {
                    var view = eunViewLst[i];

                    if (view)
                    {
                        var eunBehaviourLst = view.eunBehaviourLst;

                        for (var j = 0; j < eunBehaviourLst.Count; j++)
                        {
                            var behaviour = eunBehaviourLst[j];

                            if (behaviour) behaviour.OnEUNOtherPlayerJoinRoom(thisRoomPlayer);
                        }
                    }
                }

                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.OnEUNOtherPlayerJoinRoom(thisRoomPlayer);
                }
            }

            peer.room.LeaderClientUserId = thisRoomPlayer.UserId;

            for (var i = 0; i < eunViewLst.Count; i++)
            {
                var view = eunViewLst[i];

                if (view)
                {
                    var eunBehaviourLst = view.eunBehaviourLst;

                    for (var j = 0; j < eunBehaviourLst.Count; j++)
                    {
                        var behaviour = eunBehaviourLst[j];

                        if (behaviour) behaviour.OnEUNLeaderClientChange(thisRoomPlayer);
                    }
                }
            }

            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.OnEUNLeaderClientChange(thisRoomPlayer);
            }
        }
    }
}
