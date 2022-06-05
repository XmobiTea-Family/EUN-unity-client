namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and other room player request JoinRoom() and join this room success
    /// </summary>
    internal class OnPlayerJoinRoomEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnPlayerJoinRoom;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var roomPlayer = new RoomPlayer(parameters.GetEUNArray(ParameterCode.Data));

            var thisRoomPlayer = peer.room.roomPlayerLst.Find(x => x.userId.Equals(roomPlayer.userId));
            if (thisRoomPlayer == null)
            {
                thisRoomPlayer = roomPlayer;
                peer.room.roomPlayerLst.Add(thisRoomPlayer);

                var eunViewLst = peer.eunViewLst;
                for (var i = 0; i < eunViewLst.Count; i++)
                {
                    var view = eunViewLst[i];

                    if (view)
                    {
                        var eunBehaviourLst = view._eunBehaviourLst;

                        for (var j = 0; j < eunBehaviourLst.Count; j++)
                        {
                            var behaviour = eunBehaviourLst[j];

                            if (behaviour != null) behaviour.OnEUNOtherPlayerJoinRoom(thisRoomPlayer);
                        }
                    }
                }

                var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.OnEUNOtherPlayerJoinRoom(thisRoomPlayer);
                }
            }
        }
    }
}
