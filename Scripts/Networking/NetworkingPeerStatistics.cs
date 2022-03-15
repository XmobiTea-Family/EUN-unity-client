namespace XmobiTea.EUN.Networking
{
    public partial class NetworkingPeer
    {

        internal int GetPing()
        {
            return eunSocketObject.GetPing();
        }

        internal long GetTotalSendBytes()
        {
            return eunSocketObject.GetTotalSendBytes();
        }

        internal long GetTotalRecvBytes()
        {
            return eunSocketObject.GetTotalRecvBytes();
        }

        internal long GetTotalSendPackets()
        {
            return eunSocketObject.GetTotalSendPackets();
        }

        internal long GetTotalRecvPackets()
        {
            return eunSocketObject.GetTotalRecvPackets();
        }
    }

    public sealed class NetworkingPeerStatistics
    {
        private long lastRemoveSendBytes;
        private long lastRemoveRecvBytes;

        private long lastRemoveSendPackets;
        private long lastRemoveRecvPackets;

        private NetworkingPeer peer;

        internal NetworkingPeerStatistics(NetworkingPeer peer)
        {
            this.peer = peer;
        }

        public int GetPing()
        {
            return peer.GetPing();
        }

        public long GetSendBytes()
        {
            return peer.GetTotalSendBytes() - lastRemoveSendBytes;
        }

        public long GetRecvBytes()
        {
            return peer.GetTotalRecvBytes() - lastRemoveRecvBytes;
        }

        public void ClearBytes()
        {
            lastRemoveSendBytes = peer.GetTotalSendBytes();
            lastRemoveRecvBytes = peer.GetTotalRecvBytes();
        }

        public long GetSendPackets()
        {
            return peer.GetTotalSendPackets() - lastRemoveSendPackets;
        }

        public long GetRecvPackets()
        {
            return peer.GetTotalRecvPackets() - lastRemoveRecvPackets;
        }

        public void ClearPackets()
        {
            lastRemoveSendPackets = peer.GetTotalSendPackets();
            lastRemoveRecvPackets = peer.GetTotalRecvPackets();
        }
    }
}
