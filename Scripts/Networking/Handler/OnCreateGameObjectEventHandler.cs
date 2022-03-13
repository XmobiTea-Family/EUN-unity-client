namespace EUN.Networking
{
    using EUN.Common;
    using EUN.Constant;
    using EUN.Entity;

    internal class OnCreateGameObjectEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnCreateGameObject;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
#if EUN
            if (peer.room == null) return;
            var parameters = operationEvent.GetParameters();

            var roomGameObject = new RoomGameObject(parameters.GetEzyArray(ParameterCode.Data));
            peer.room.GameObjectDic[roomGameObject.ObjectId] = roomGameObject;

            foreach (var behaviour in peer.ezyManagerBehaviourLst)
            {
                if (behaviour)
                {
                    var view = behaviour.OnEzyViewNeedCreate(roomGameObject);
                    if (view != null)
                    {
                        view.Init(roomGameObject);
                        peer.ezyViewDic[view.RoomGameObject.ObjectId] = view;
                    }
                }
            }
#endif
        }
    }
}