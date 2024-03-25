namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and other room player send LeaveRoom() and left this room success
    /// </summary>
    internal class OnPlayerLeftRoomEventHandler : IServerEventHandler
    {
        public int getEventCode()
        {
            return EventCode.OnPlayerLeftRoom;
        }

        public void handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.getParameters();
            var roomPlayer = new RoomPlayer(parameters.getEUNArray(ParameterCode.Data));

            var thisRoomPlayer = peer.room.roomPlayerLst.Find(x => x.userId.Equals(roomPlayer.userId));
            if (thisRoomPlayer != null)
            {
                peer.room.roomPlayerLst.Remove(thisRoomPlayer);

                foreach (var view in peer.eunViewLst)
                {
                    if (view)
                    {
                        foreach (var behaviour in view.eunBehaviourLst)
                        {
                            if (behaviour) behaviour.onEUNOtherPlayerLeftRoom(thisRoomPlayer);
                        }
                    }
                }

                var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.onEUNOtherPlayerLeftRoom(thisRoomPlayer);
                }
            }
        }

    }

}
