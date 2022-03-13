namespace EUN.Networking
{
    using EUN.Common;
    using EUN.Constant;

    internal class OnVoiceChatEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnVoiceChat;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
#if EUN
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var ezyArray = parameters.GetEzyArray(ParameterCode.Data);

            var objectId = ezyArray.get<int>(0);
            var voiceChatData = ezyArray.get<object>(1);

            if (peer.ezyViewDic.ContainsKey(objectId))
            {
                var view = peer.ezyViewDic[objectId];
                if (view)
                {
                    foreach (var behaviour in view.ezyVoiceChatBehaviourLst)
                    {
                        if (behaviour) behaviour.OnEzySynchronization(voiceChatData);
                    }
                }
            }
#endif
        }
    }
}