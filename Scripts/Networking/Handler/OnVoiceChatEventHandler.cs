namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and eunView send voice chat success
    /// </summary>
    internal class OnVoiceChatEventHandler : IServerEventHandler
    {
        public int getEventCode()
        {
            return EventCode.OnVoiceChat;
        }

        public void handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.getParameters();
            var eunArray = parameters.getEUNArray(ParameterCode.Data);

            var objectId = eunArray.getInt(0);
            var voiceChatData = eunArray.getObject(1);

            if (peer.eunViewDict.ContainsKey(objectId))
            {
                var view = peer.eunViewDict[objectId];
                if (view)
                {
                    foreach (var behaviour in view.eunVoiceChatBehaviourLst)
                    {
                        if (behaviour != null) behaviour.onEUNSynchronization(voiceChatData);
                    }
                }
            }
        }

    }

}
