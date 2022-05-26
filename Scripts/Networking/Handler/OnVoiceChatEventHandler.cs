namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and eunView send voice chat success
    /// </summary>
    internal class OnVoiceChatEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnVoiceChat;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var eunArray = parameters.GetEUNArray(ParameterCode.Data);

            var objectId = eunArray.GetInt(0);
            var voiceChatData = eunArray.GetObject(1);

            if (peer.eunViewDic.ContainsKey(objectId))
            {
                var view = peer.eunViewDic[objectId];
                if (view)
                {
                    foreach (var behaviour in view.eunVoiceChatBehaviourLst)
                    {
                        if (behaviour != null) behaviour.OnEUNSynchronization(voiceChatData);
                    }
                }
            }
        }
    }
}
