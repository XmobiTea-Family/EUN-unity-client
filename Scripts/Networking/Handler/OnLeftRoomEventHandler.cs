namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    internal class OnLeftRoomEventHandler : IServerEventHandler
    {
        public int GetEventCode()
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
