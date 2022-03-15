namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    internal class OnVoiceChatEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnVoiceChat;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var customArray = parameters.GetCustomArray(ParameterCode.Data);

            var objectId = customArray.GetInt(0);
            var voiceChatData = customArray.GetObject(1);

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
        }
    }
}