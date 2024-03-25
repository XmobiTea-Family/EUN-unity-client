namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client subscriber chat all and someone send ChatAll() request
    /// </summary>
    internal class OnChatAllEventHandler : IServerEventHandler
    {
        public int getEventCode()
        {
            return EventCode.OnChatAll;
        }

        public void handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            var parameters = operationEvent.getParameters();
            var message = new ChatMessage(parameters.getEUNArray(ParameterCode.Message));

            var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.onEUNReceiveChatAll(message);
            }
        }

    }

}
