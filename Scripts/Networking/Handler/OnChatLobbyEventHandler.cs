namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    internal class OnChatLobbyEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnChatLobby;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            var parameters = operationEvent.GetParameters();
            var message = new ChatMessage(parameters.GetEUNArray(ParameterCode.Message));

            var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.OnEUNReceiveChatLobby(message);
            }
        }
    }
}
