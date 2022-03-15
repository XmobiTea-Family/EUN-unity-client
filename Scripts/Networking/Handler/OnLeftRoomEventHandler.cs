namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    internal class OnLeftRoomEventHandler : IServerEventHandler
    {
        public EventCode GetEventCode()
        {
            return EventCode.OnLeftRoom;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            peer.room = null;

            foreach (var behaviour in peer.eunManagerBehaviourLst)
            {
                if (behaviour) behaviour.OnEUNLeftRoom();
            }
        }
    }
}