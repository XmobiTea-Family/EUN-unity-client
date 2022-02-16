namespace EUN.Networking
{
    using EUN.Common;
    using EUN.Constant;
    using EUN.Entity;

    internal class OnPlayerJoinRoomEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnPlayerJoinRoom;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var roomPlayer = new RoomPlayer(parameters.GetEzyArray(ParameterCode.Data));

            var thisRoomPlayer = peer.room.RoomPlayerLst.Find(x => x.UserId.Equals(roomPlayer.UserId));
            if (thisRoomPlayer == null)
            {
                thisRoomPlayer = roomPlayer;
                peer.room.RoomPlayerLst.Add(thisRoomPlayer);

                foreach (var view in peer.ezyViewLst)
                {
                    if (view)
                    {
                        foreach (var behaviour in view.ezyBehaviourLst)
                        {
                            if (behaviour) behaviour.OnEzyOtherPlayerJoinRoom(thisRoomPlayer);
                        }
                    }
                }

                foreach (var behaviour in peer.ezyManagerBehaviourLst)
                {
                    if (behaviour) behaviour.OnEzyOtherPlayerJoinRoom(thisRoomPlayer);
                }
            }
        }
    }
}