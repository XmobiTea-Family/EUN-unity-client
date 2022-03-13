namespace EUN.Networking
{
    using EUN.Common;
    using EUN.Constant;
    using EUN.Entity;

    internal class OnPlayerLeftRoomEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnPlayerLeftRoom;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
#if EUN
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var roomPlayer = new RoomPlayer(parameters.GetEzyArray(ParameterCode.Data));

            var thisRoomPlayer = peer.room.RoomPlayerLst.Find(x => x.UserId.Equals(roomPlayer.UserId));
            if (thisRoomPlayer != null)
            {
                peer.room.RoomPlayerLst.Remove(thisRoomPlayer);

                foreach (var view in peer.ezyViewLst)
                {
                    if (view)
                    {
                        foreach (var behaviour in view.ezyBehaviourLst)
                        {
                            if (behaviour) behaviour.OnEzyOtherPlayerLeftRoom(thisRoomPlayer);
                        }
                    }
                }

                foreach (var behaviour in peer.ezyManagerBehaviourLst)
                {
                    if (behaviour) behaviour.OnEzyOtherPlayerLeftRoom(thisRoomPlayer);
                }
            }
#endif
        }
    }
}