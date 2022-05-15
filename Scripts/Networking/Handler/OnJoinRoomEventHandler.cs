namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    internal class OnJoinRoomEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnJoinRoom;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            var parameters = operationEvent.GetParameters();
            var room = new Room(parameters.GetEUNArray(ParameterCode.Data));
            peer.room = room;

            var roomPlayer = room.RoomPlayerLst.Find(x => x.UserId.Equals(EUNNetwork.UserId));
            peer.playerId = roomPlayer == null ? -1 : roomPlayer.PlayerId;

            foreach (var behaviour in peer.eunManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEUNJoinRoom();
            }
        }
    }
}
