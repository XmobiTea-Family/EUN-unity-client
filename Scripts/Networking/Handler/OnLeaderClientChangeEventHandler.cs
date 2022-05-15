namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    internal class OnLeaderClientChangeEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnLeaderClientChange;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var roomPlayer = new RoomPlayer(parameters.GetEUNArray(ParameterCode.Data));

            var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;

            var thisRoomPlayer = peer.room.RoomPlayerLst.Find(x => x.UserId.Equals(roomPlayer.UserId));
            if (thisRoomPlayer == null)
            {
                thisRoomPlayer = roomPlayer;
                peer.room.RoomPlayerLst.Add(thisRoomPlayer);

                foreach (var view in peer.eunViewLst)
                {
                    if (view)
                    {
                        foreach (var behaviour in view.eunBehaviourLst)
                        {
                            if (behaviour) behaviour.OnEUNOtherPlayerJoinRoom(thisRoomPlayer);
                        }
                    }
                }

                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null) behaviour.OnEUNOtherPlayerJoinRoom(thisRoomPlayer);
                }
            }

            peer.room.LeaderClientUserId = thisRoomPlayer.UserId;

            foreach (var view in peer.eunViewLst)
            {
                if (view)
                {
                    foreach (var behaviour in view.eunBehaviourLst)
                    {
                        if (behaviour) behaviour.OnEUNLeaderClientChange(thisRoomPlayer);
                    }
                }
            }

            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.OnEUNLeaderClientChange(thisRoomPlayer);
            }
        }
    }
}
