namespace EUN.Networking
{
    using EUN.Common;
    using EUN.Constant;
    using EUN.Entity;

    internal class OnChatRoomEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnChatRoom;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
#if EUN
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var message = new ChatMessage(parameters.GetEzyArray(ParameterCode.Message));

            var thisRoomPlayer = peer.room.RoomPlayerLst.Find(x => x.UserId.Equals(message.SenderId));
            if (thisRoomPlayer != null)
            {
                foreach (var view in peer.ezyViewLst)
                {
                    if (view)
                    {
                        foreach (var behaviour in view.ezyBehaviourLst)
                        {
                            if (behaviour) behaviour.OnEzyReceiveChatRoom(message, thisRoomPlayer);
                        }
                    }
                }

                foreach (var behaviour in peer.ezyManagerBehaviourLst)
                {
                    if (behaviour) behaviour.OnEzyReceiveChatRoom(message, thisRoomPlayer);
                }
            }
#endif
        }
    }
}