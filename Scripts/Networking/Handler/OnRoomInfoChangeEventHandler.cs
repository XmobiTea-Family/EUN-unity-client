namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Constant;

    /// <summary>
    /// Handle if client inroom and leader client change room info, or room custom properties this room success
    /// </summary>
    internal class OnRoomInfoChangeEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnRoomInfoChange;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;
            var currentRoom = peer.room;

            var parameters = operationEvent.GetParameters();

            if (parameters.ContainsKey(ParameterCode.CustomRoomProperties))
            {
                var data = parameters.GetEUNHashtable(ParameterCode.CustomRoomProperties);
                var keySet = data.Keys();
                foreach (int key in keySet)
                {
                    var value = data.GetObject(key);

                    if (value == null)
                    {
                        if (currentRoom.CustomRoomProperties.ContainsKey(key))
                        {
                            currentRoom.CustomRoomProperties.Remove(key);
                        }
                    }
                    else currentRoom.CustomRoomProperties.Add(key, value);
                }

                foreach (var view in peer.eunViewLst)
                {
                    if (view)
                    {
                        foreach (var behaviour in view.eunBehaviourLst)
                        {
                            if (behaviour != null) behaviour.OnEUNCustomRoomPropertiesChange(data);
                        }
                    }
                }

                var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.OnEUNCustomRoomPropertiesChange(data);
                }

                parameters.Remove(ParameterCode.CustomRoomProperties);
            }

            var canNotify = false;

            if (parameters.ContainsKey(ParameterCode.CustomRoomPropertiesForLobby))
            {
                var data = parameters.GetEUNArray(ParameterCode.CustomRoomPropertiesForLobby);
                currentRoom.CustomRoomPropertiesForLobby.Clear();
                {
                    for (var i = 0; i < data.Count(); i++)
                    {
                        currentRoom.CustomRoomPropertiesForLobby.Add(data.GetInt(i));
                    }
                }

                canNotify = true;
            }

            if (parameters.ContainsKey(ParameterCode.MaxPlayer))
            {
                var data = parameters.GetInt(ParameterCode.MaxPlayer);
                currentRoom.MaxPlayer = data;

                canNotify = true;
            }

            if (parameters.ContainsKey(ParameterCode.IsOpen))
            {
                var data = parameters.GetBool(ParameterCode.IsOpen);
                currentRoom.IsOpen = data;

                canNotify = true;
            }

            if (parameters.ContainsKey(ParameterCode.IsVisible))
            {
                var data = parameters.GetBool(ParameterCode.IsVisible);
                currentRoom.IsVisible = data;

                canNotify = true;
            }

            if (parameters.ContainsKey(ParameterCode.Password))
            {
                var data = parameters.GetString(ParameterCode.Password);
                currentRoom.Password = data;

                canNotify = true;
            }

            if (parameters.ContainsKey(ParameterCode.Ttl))
            {
                var data = parameters.GetInt(ParameterCode.Ttl);
                currentRoom.Ttl = data;

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
                            if (behaviour != null) behaviour.OnEUNRoomInfoChange(parameters);
                        }
                    }
                }

                var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.OnEUNRoomInfoChange(parameters);
                }
            }
        }
    }
}
