namespace EUN.Networking
{
    using EUN.Common;
    using EUN.Constant;

    internal class OnDestroyGameObjectEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnDestroyGameObject;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
#if EUN
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var ezyArray = parameters.GetEzyArray(ParameterCode.Data);
            var objectId = ezyArray.get<int>(0);

            if (peer.room.GameObjectDic.ContainsKey(objectId)) peer.room.GameObjectDic.Remove(objectId);
#endif
        }
    }
}