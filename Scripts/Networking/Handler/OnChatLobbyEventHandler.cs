namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    internal class OnChatLobbyEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnChatLobby;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            var parameters = operationEvent.GetParameters();
            var message = new ChatMessage(parameters.GetEUNArray(ParameterCode.Message));

            foreach (var behaviour in peer.eunManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEUNReceiveChatLobby(message);
            }
        }
    }
}