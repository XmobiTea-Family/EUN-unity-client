namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    internal class OnChatRoomEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnChatRoom;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var message = new ChatMessage(parameters.GetEUNArray(ParameterCode.Message));

            var thisRoomPlayer = peer.room.RoomPlayerLst.Find(x => x.UserId.Equals(message.SenderId));
            if (thisRoomPlayer != null)
            {
                foreach (var view in peer.eunViewLst)
                {
                    if (view)
                    {
                        foreach (var behaviour in view.eunBehaviourLst)
                        {
                            if (behaviour) behaviour.OnEUNReceiveChatRoom(message, thisRoomPlayer);
                        }
                    }
                }

                foreach (var behaviour in peer.eunManagerBehaviourLst)
                {
                    if (behaviour) behaviour.OnEUNReceiveChatRoom(message, thisRoomPlayer);
                }
            }
        }
    }
}