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

            var thisRoomPlayer = peer.room.RoomPlayerLst.Find(x => x.PlayerId == playerId);
            if (thisRoomPlayer != null)
            {
                var customPlayerProperties = eunArray.GetEUNHashtable(1);
                var keySet = customPlayerProperties.Keys();
                foreach (int key in keySet)
                {
                    var value = customPlayerProperties.GetObject(key);

                    if (value == null)
                    {
                        if (thisRoomPlayer.CustomProperties.ContainsKey(key))
                        {
                            thisRoomPlayer.CustomProperties.Remove(key);
                        }
                    }
                    else thisRoomPlayer.CustomProperties.Add(key, value);
                }

                foreach (var view in peer.eunViewLst)
                {
                    if (view)
                    {
                        foreach (var behaviour in view.eunBehaviourLst)
                        {
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
