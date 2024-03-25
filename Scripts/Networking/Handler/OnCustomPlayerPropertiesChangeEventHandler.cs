namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and someone inroom send ChangePlayerCustomProperties() request
    /// </summary>
    internal class OnPlayerCustomPropertiesChangeEventHandler : IServerEventHandler
    {
        public int getEventCode()
        {
            return EventCode.OnPlayerCustomPropertiesChange;
        }

        public void handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.getParameters();
            var eunArray = parameters.getEUNArray(ParameterCode.Data);
            var playerId = eunArray.getInt(0);

            var thisRoomPlayer = peer.room.roomPlayerLst.Find(x => x.playerId == playerId);
            if (thisRoomPlayer != null)
            {
                var customPlayerProperties = eunArray.getEUNHashtable(1);
                var keySet = customPlayerProperties.keys();
                foreach (int key in keySet)
                {
                    var value = customPlayerProperties.getObject(key);

                    if (value == null)
                    {
                        if (thisRoomPlayer.customProperties.containsKey(key))
                        {
                            thisRoomPlayer.customProperties.remove(key);
                        }
                    }
                    else thisRoomPlayer.customProperties.add(key, value);
                }

                foreach (var view in peer.eunViewLst)
                {
                    if (view)
                    {
                        foreach (var behaviour in view.eunBehaviourLst)
                        {
                            if (behaviour != null) behaviour.onEUNCustomPlayerPropertiesChange(thisRoomPlayer, customPlayerProperties);
                        }
                    }
                }

                var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.onEUNCustomPlayerPropertiesChange(thisRoomPlayer, customPlayerProperties);
                }
            }
        }

    }

}
