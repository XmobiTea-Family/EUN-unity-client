namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and someone inroom send ChatRoom() request
    /// </summary>
    internal class OnChatRoomEventHandler : IServerEventHandler
    {
        public int getEventCode()
        {
            return EventCode.OnChatRoom;
        }

        public void handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.getParameters();
            var message = new ChatMessage(parameters.getEUNArray(ParameterCode.Message));

            var thisRoomPlayer = peer.room.roomPlayerLst.Find(x => x.userId.Equals(message.senderId));
            if (thisRoomPlayer != null)
            {
                foreach (var view in peer.eunViewLst)
                {
                    if (view)
                    {
                        foreach (var behaviour in view.eunBehaviourLst)
                        {
                            if (behaviour != null) behaviour.onEUNReceiveChatRoom(message, thisRoomPlayer);
                        }
                    }
                }

                var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.onEUNReceiveChatRoom(message, thisRoomPlayer);
                }
            }
        }

    }

}
