namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Constant;

    /// <summary>
    /// Handle if client inroom and leader client change room info, or room custom properties this room success
    /// </summary>
    internal class OnRoomInfoChangeEventHandler : IServerEventHandler
    {
        public int getEventCode()
        {
            return EventCode.OnRoomInfoChange;
        }

        public void handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;
            var currentRoom = peer.room;

            var parameters = operationEvent.getParameters();

            if (parameters.containsKey(ParameterCode.CustomRoomProperties))
            {
                var data = parameters.getEUNHashtable(ParameterCode.CustomRoomProperties);
                var keySet = data.keys();
                foreach (int key in keySet)
                {
                    var value = data.getObject(key);

                    if (value == null)
                    {
                        if (currentRoom.customRoomProperties.containsKey(key))
                        {
                            currentRoom.customRoomProperties.remove(key);
                        }
                    }
                    else currentRoom.customRoomProperties.add(key, value);
                }

                foreach (var view in peer.eunViewLst)
                {
                    if (view)
                    {
                        foreach (var behaviour in view.eunBehaviourLst)
                        {
                            if (behaviour != null) behaviour.onEUNCustomRoomPropertiesChange(data);
                        }
                    }
                }

                var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.onEUNCustomRoomPropertiesChange(data);
                }

                parameters.remove(ParameterCode.CustomRoomProperties);
            }

            var canNotify = false;

            if (parameters.containsKey(ParameterCode.CustomRoomPropertiesForLobby))
            {
                var data = parameters.getEUNArray(ParameterCode.CustomRoomPropertiesForLobby);
                currentRoom.customRoomPropertiesForLobby.Clear();
                {
                    for (var i = 0; i < data.count(); i++)
                    {
                        currentRoom.customRoomPropertiesForLobby.Add(data.getInt(i));
                    }
                }

                canNotify = true;
            }

            if (parameters.containsKey(ParameterCode.MaxPlayer))
            {
                var data = parameters.getInt(ParameterCode.MaxPlayer);
                currentRoom.maxPlayer = data;

                canNotify = true;
            }

            if (parameters.containsKey(ParameterCode.IsOpen))
            {
                var data = parameters.getBool(ParameterCode.IsOpen);
                currentRoom.isOpen = data;

                canNotify = true;
            }

            if (parameters.containsKey(ParameterCode.IsVisible))
            {
                var data = parameters.getBool(ParameterCode.IsVisible);
                currentRoom.isVisible = data;

                canNotify = true;
            }

            if (parameters.containsKey(ParameterCode.Password))
            {
                var data = parameters.getString(ParameterCode.Password);
                currentRoom.password = data;

                canNotify = true;
            }

            if (parameters.containsKey(ParameterCode.Ttl))
            {
                var data = parameters.getInt(ParameterCode.Ttl);
                currentRoom.ttl = data;

                canNotify = true;
            }

            if (canNotify)
            {
                foreach (var view in peer.eunViewLst)
                {
                    if (view)
                    {
                        foreach (var behaviour in view.eunBehaviourLst)
                        {
                            if (behaviour != null) behaviour.onEUNRoomInfoChange(parameters);
                        }
                    }
                }

                var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.onEUNRoomInfoChange(parameters);
                }
            }
        }

    }

}
