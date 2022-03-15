namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    internal class OnJoinRoomEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnJoinRoom;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            var parameters = operationEvent.GetParameters();
            var room = new Room(parameters.GetCustomArray(ParameterCode.Data));
            peer.room = room;

            foreach (var behaviour in peer.ezyManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEzyJoinRoom();
            }

            var roomPlayer = room.RoomPlayerLst.Find(x => x.UserId.Equals(EzyNetwork.UserId));

            peer.playerId = roomPlayer == null ? -1 : roomPlayer.PlayerId;
        }
    }
}