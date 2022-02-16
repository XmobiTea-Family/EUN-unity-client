namespace EUN.Networking
{
    using EUN.Common;
    using EUN.Constant;
    using EUN.Entity;

    internal class OnChatAllEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnChatAll;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            var parameters = operationEvent.GetParameters();
            var message = new ChatMessage(parameters.GetEzyArray(ParameterCode.Message));

            foreach (var behaviour in peer.ezyManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEzyReceiveChatAll(message);
            }
        }
    }
}