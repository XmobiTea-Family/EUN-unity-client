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
        public int getEventCode()
        {
            return EventCode.OnLeaderClientChange;
        }

        public void handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.getParameters();
            var roomPlayer = new RoomPlayer(parameters.getEUNArray(ParameterCode.Data));

            var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;

            var thisRoomPlayer = peer.room.roomPlayerLst.Find(x => x.userId.Equals(roomPlayer.userId));
            if (thisRoomPlayer == null)
            {
                thisRoomPlayer = roomPlayer;
                peer.room.roomPlayerLst.Add(thisRoomPlayer);

                foreach (var view in peer.eunViewLst)
                {
                    if (view)
                    {
                        foreach (var behaviour in view.eunBehaviourLst)
                        {
                            if (behaviour) behaviour.onEUNOtherPlayerJoinRoom(thisRoomPlayer);
                        }
                    }
                }

                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.onEUNOtherPlayerJoinRoom(thisRoomPlayer);
                }
            }

            peer.room.leaderClientUserId = thisRoomPlayer.userId;

            foreach (var view in peer.eunViewLst)
            {
                if (view)
                {
                    foreach (var behaviour in view.eunBehaviourLst)
                    {
                        if (behaviour) behaviour.onEUNLeaderClientChange(thisRoomPlayer);
                    }
                }
            }

            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.onEUNLeaderClientChange(thisRoomPlayer);
            }
        }

    }

}
