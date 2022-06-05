namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and someone inroom send ChatRoom() request
    /// </summary>
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

                            if (behaviour != null) behaviour.OnEUNReceiveChatRoom(message, thisRoomPlayer);
                        }
                    }
                }

                var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.OnEUNReceiveChatRoom(message, thisRoomPlayer);
                }
            }
        }
    }
}
