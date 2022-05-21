namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    internal class OnCreateGameObjectEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnCreateGameObject;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;
            var parameters = operationEvent.GetParameters();

            var roomGameObject = new RoomGameObject(parameters.GetEUNArray(ParameterCode.Data));
            peer.room.GameObjectDic[roomGameObject.ObjectId] = roomGameObject;
            
            var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null)
                {
                    var view = behaviour.OnEUNViewNeedCreate(roomGameObject);
                    if (view != null)
                    {
                        view.Init(roomGameObject);
                        peer.eunViewDic[view.RoomGameObject.ObjectId] = view;
                    }
                }
            }
        }
    }
}
