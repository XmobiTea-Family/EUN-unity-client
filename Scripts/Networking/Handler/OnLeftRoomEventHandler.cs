namespace EUN.Networking
{
    using EUN.Common;
    using EUN.Constant;

    internal class OnLeftRoomEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnLeftRoom;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            peer.room = null;

            foreach (var behaviour in peer.ezyManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEzyLeftRoom();
            }
        }
    }
}