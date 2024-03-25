namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and other room player request JoinRoom() and join this room success
    /// </summary>
    internal class OnPlayerJoinRoomEventHandler : IServerEventHandler
    {
        public int getEventCode()
        {
            return EventCode.OnPlayerJoinRoom;
        }

        public void handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.getParameters();
            var roomPlayer = new RoomPlayer(parameters.getEUNArray(ParameterCode.Data));

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
                            if (behaviour != null) behaviour.onEUNOtherPlayerJoinRoom(thisRoomPlayer);
                        }
                    }
                }

                var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.onEUNOtherPlayerJoinRoom(thisRoomPlayer);
                }
            }
        }

    }

}
