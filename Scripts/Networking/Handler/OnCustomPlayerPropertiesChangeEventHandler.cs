namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and someone inroom send ChangePlayerCustomProperties() request
    /// </summary>
    internal class OnCustomPlayerPropertiesChangeEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnCustomPlayerPropertiesChange;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var eunArray = parameters.GetEUNArray(ParameterCode.Data);
            var playerId = eunArray.GetInt(0);

            var thisRoomPlayer = peer.room.roomPlayerLst.Find(x => x.playerId == playerId);
            if (thisRoomPlayer != null)
            {
                var customPlayerProperties = eunArray.GetEUNHashtable(1);
                var keySet = customPlayerProperties.Keys();
                foreach (int key in keySet)
                {
                    var value = customPlayerProperties.GetObject(key);

                    if (value == null)
                    {
                        if (thisRoomPlayer.customProperties.ContainsKey(key))
                        {
                            thisRoomPlayer.customProperties.Remove(key);
                        }
                    }
                    else thisRoomPlayer.customProperties.Add(key, value);
                }

                var eunViewLst = peer.eunViewLst;
                for (var i = 0; i < eunViewLst.Count; i++)
                {
                    var view = eunViewLst[i];

                    if (view)
                    {
                        var eunBehaviourLst = view._eunBehaviourLst;

                        for (var j = 0; j < eunBehaviourLst.Count; j++)
                        {
                            var behaviour = eunBehaviourLst[j];

                            if (behaviour != null) behaviour.OnEUNCustomPlayerPropertiesChange(thisRoomPlayer, customPlayerProperties);
                        }
                    }
                }

                var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.OnEUNCustomPlayerPropertiesChange(thisRoomPlayer, customPlayerProperties);
                }
            }
        }
    }
}
