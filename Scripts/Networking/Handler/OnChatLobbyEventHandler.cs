namespace EUN.Networking
{
    using EUN.Common;
    using EUN.Constant;
    using EUN.Entity;

    internal class OnChatLobbyEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnChatLobby;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            var parameters = operationEvent.GetParameters();
            var message = new ChatMessage(parameters.GetCustomArray(ParameterCode.Message));

            foreach (var behaviour in peer.ezyManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEzyReceiveChatLobby(message);
            }
        }
    }
}