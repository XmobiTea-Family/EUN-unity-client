namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and other room player send LeaveRoom() and left this room success
    /// </summary>
    internal class OnPlayerLeftRoomEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnPlayerLeftRoom;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var roomPlayer = new RoomPlayer(parameters.GetEUNArray(ParameterCode.Data));

            var thisRoomPlayer = peer.room.RoomPlayerLst.Find(x => x.UserId.Equals(roomPlayer.UserId));
            if (thisRoomPlayer != null)
            {
                peer.room.RoomPlayerLst.Remove(thisRoomPlayer);

                var eunViewLst = peer.eunViewLst;

                for (var i = 0; i < eunViewLst.Count; i++)
                {
                    var view = eunViewLst[i];

                    if (view)
                    {
                        var eunBehaviourLst = view.eunBehaviourLst;

                        for (var j = 0; j < eunBehaviourLst.Count; j++)
                        {
                            var behaviour = eunBehaviourLst[j];

                            if (behaviour) behaviour.OnEUNOtherPlayerLeftRoom(thisRoomPlayer);
                        }
                    }
                }

                var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.OnEUNOtherPlayerLeftRoom(thisRoomPlayer);
                }
            }
        }
    }
}
