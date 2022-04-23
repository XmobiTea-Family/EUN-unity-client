namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

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

            var thisRoomPlayer = peer.room.RoomPlayerLst.Find(x => x.UserId.Equals(roomPlayer.UserId));
            if (thisRoomPlayer == null)
            {
                thisRoomPlayer = roomPlayer;
                peer.room.RoomPlayerLst.Add(thisRoomPlayer);

                foreach (var view in peer.eunViewLst)
                {
                    if (view)
                    {
                        foreach (var behaviour in view.eunBehaviourLst)
                        {
                            if (behaviour) behaviour.OnEUNOtherPlayerJoinRoom(thisRoomPlayer);
                        }
                    }
                }

                foreach (var behaviour in peer.eunManagerBehaviourLst)
                {
                    if (behaviour) behaviour.OnEUNOtherPlayerJoinRoom(thisRoomPlayer);
                }
            }
        }
    }
}