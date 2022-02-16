namespace EUN.Networking
{
    using EUN.Common;
    using EUN.Constant;

    internal class OnTransferOwnerGameObjectEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnTransferOwnerGameObject;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            if (peer.room == null) return;

            var parameters = operationEvent.GetParameters();
            var ezyArray = parameters.GetEzyArray(ParameterCode.Data);

            var objectId = ezyArray.get<int>(0);
            var newOwnerId = ezyArray.get<int>(1);

            if (peer.room.GameObjectDic.ContainsKey(objectId))
            {
                peer.room.GameObjectDic[objectId].OwnerId = newOwnerId;
            }
        }
    }
}