namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// Handle if client inroom and send LeaveRoom() success
    /// </summary>
    internal class OnLeftRoomEventHandler : IServerEventHandler
    {
        public int GetEventCode()
        {
            return EventCode.OnLeftRoom;
        }

        public void Handle(OperationEvent operationEvent, NetworkingPeer peer)
        {
            peer.room = null;

            var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
            for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
            {
                var behaviour = eunManagerBehaviourLst[i];
                if (behaviour != null) behaviour.OnEUNLeftRoom();
            }
        }
    }
}
