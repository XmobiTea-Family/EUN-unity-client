namespace EUN.Networking
{
    using EUN.Common;
    using EUN.Constant;

    internal class OnRoomInfoChangeEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
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
                var data = parameters.GetCustomHashtable(ParameterCode.CustomRoomProperties);
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

                foreach (var view in peer.ezyViewLst)
                {
                    if (view)
                    {
                        foreach (var behaviour in view.ezyBehaviourLst)
                        {
                            if (behaviour) behaviour.OnEzyCustomRoomPropertiesChange(data);
                        }
                    }
                }

                foreach (var behaviour in peer.ezyManagerBehaviourLst)
                {
                    if (behaviour) behaviour.OnEzyCustomRoomPropertiesChange(data);
                }

                parameters.Remove(ParameterCode.CustomRoomProperties);
            }

            var canNotify = false;

            if (parameters.ContainsKey(ParameterCode.CustomRoomPropertiesForLobby))
            {
                var data = parameters.GetCustomArray(ParameterCode.CustomRoomPropertiesForLobby);
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
                foreach (var view in peer.ezyViewLst)
                {
                    if (view)
                    {
                        foreach (var behaviour in view.ezyBehaviourLst)
                        {
                            if (behaviour) behaviour.OnEzyRoomInfoChange(parameters);
                        }
                    }
                }

                foreach (var behaviour in peer.ezyManagerBehaviourLst)
                {
                    if (behaviour) behaviour.OnEzyRoomInfoChange(parameters);
                }
            }
        }
    }
}