namespace EUN.Networking
{
    using com.tvd12.ezyfoxserver.client.entity;

    using EUN.Common;
    using EUN.Constant;

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
            var ezyArray = parameters.GetEzyArray(ParameterCode.Data);
            var playerId = ezyArray.get<int>(0);

            var thisRoomPlayer = peer.room.RoomPlayerLst.Find(x => x.PlayerId == playerId);
            if (thisRoomPlayer != null)
            {
                var customPlayerProperties = new CustomHashtable(ezyArray.get<EzyObject>(1));
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

                foreach (var view in peer.ezyViewLst)
                {
                    if (view)
                    {
                        foreach (var behaviour in view.ezyBehaviourLst)
                        {
                            if (behaviour) behaviour.OnEzyCustomPlayerPropertiesChange(thisRoomPlayer, customPlayerProperties);
                        }
                    }
                }

                foreach (var behaviour in peer.ezyManagerBehaviourLst)
                {
                    if (behaviour) behaviour.OnEzyCustomPlayerPropertiesChange(thisRoomPlayer, customPlayerProperties);
                }
            }
        }
    }
}