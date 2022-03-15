namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    internal class OnCustomPlayerPropertiesChangeEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
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
                            if (behaviour) behaviour.OnEUNCustomPlayerPropertiesChange(thisRoomPlayer, customPlayerProperties);
                        }
                    }
                }

                foreach (var behaviour in peer.eunManagerBehaviourLst)
                {
                    if (behaviour) behaviour.OnEUNCustomPlayerPropertiesChange(thisRoomPlayer, customPlayerProperties);
                }
            }
        }
    }
}