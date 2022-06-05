﻿namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client are not inroom and send JoinRoom(), CreateRoom(), JoinRandomRoom() or JoinOrCreateRoom() success
    /// </summary>
    internal class OnJoinRoomEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnJoinRoom;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            var parameters = operationEvent.GetParameters();
            var room = new Room(parameters.GetEUNArray(ParameterCode.Data));
            peer.room = room;

            var roomPlayer = room.roomPlayerLst.Find(x => x.userId.Equals(EUNNetwork.UserId));
            peer.playerId = roomPlayer == null ? -1 : roomPlayer.playerId;

            var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.OnEUNJoinRoom();
            }

            var roomGameObjectNeedCreateLst = peer.getListGameObjectNeedCreate();
            if (roomGameObjectNeedCreateLst != null && roomGameObjectNeedCreateLst.Count != 0)
            {
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null)
                    {
                        for (var j = 0; j < roomGameObjectNeedCreateLst.Count; j++)
                        {
                            var roomGameObject = roomGameObjectNeedCreateLst[j];

                            var view = behaviour.OnEUNViewNeedCreate(roomGameObject);

                            if (view != null)
                            {
                                view.Init(roomGameObject);
                            }
                        }
                    }
                }
            }
        }
    }
}
