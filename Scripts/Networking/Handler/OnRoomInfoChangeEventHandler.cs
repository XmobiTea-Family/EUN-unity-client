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
                        if (currentRoom.customRoomProperties.ContainsKey(key))
                        {
                            currentRoom.customRoomProperties.Remove(key);
                        }
                    }
                    else currentRoom.customRoomProperties.Add(key, value);
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
                currentRoom.customRoomPropertiesForLobbyLst.Clear();
                {
                    for (var i = 0; i < data.Count(); i++)
                    {
                        currentRoom.customRoomPropertiesForLobbyLst.Add(data.GetInt(i));
                    }
                }

                canNotify = true;
            }

            if (parameters.ContainsKey(ParameterCode.MaxPlayer))
            {
                var data = parameters.GetInt(ParameterCode.MaxPlayer);
                currentRoom.maxPlayer = data;

                canNotify = true;
            }

            if (parameters.ContainsKey(ParameterCode.IsOpen))
            {
                var data = parameters.GetBool(ParameterCode.IsOpen);
                currentRoom.isOpen = data;

                canNotify = true;
            }

            if (parameters.ContainsKey(ParameterCode.IsVisible))
            {
                var data = parameters.GetBool(ParameterCode.IsVisible);
                currentRoom.isVisible = data;

                canNotify = true;
            }

            if (parameters.ContainsKey(ParameterCode.Password))
            {
                var data = parameters.GetString(ParameterCode.Password);
                currentRoom.password = data;

                canNotify = true;
            }

            if (parameters.ContainsKey(ParameterCode.Ttl))
            {
                var data = parameters.GetInt(ParameterCode.Ttl);
                currentRoom.ttl = data;

                canNotify = true;
            }

            if (canNotify)
            {
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
