namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    internal class OnDestroyGameObjectEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnDestroyGameObject;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var eunArray = parameters.GetEUNArray(ParameterCode.Data);
            var objectId = eunArray.GetInt(0);

            if (peer.room.GameObjectDic.ContainsKey(objectId)) peer.room.GameObjectDic.Remove(objectId);
        }
    }
}