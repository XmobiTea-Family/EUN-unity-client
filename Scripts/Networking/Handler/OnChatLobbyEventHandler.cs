namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client subscriber chat lobby and someone send ChatLobby() to current lobby request
    /// </summary>
    internal class OnChatLobbyEventHandler : IServerEventHandler
    {
        public int getEventCode()
        {
            return EventCode.OnChatLobby;
        }

        public void handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            var parameters = operationEvent.getParameters();
            var message = new ChatMessage(parameters.getEUNArray(ParameterCode.Message));

            var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.onEUNReceiveChatLobby(message);
            }
        }

    }

}
