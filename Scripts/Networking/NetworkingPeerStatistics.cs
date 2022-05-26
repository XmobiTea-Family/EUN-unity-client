namespace XmobiTea.EUN.Networking
{
    /// <summary>
    /// In this partial it will contains all network statistics
    /// </summary>
    public partial class NetworkingPeer
    {
        /// <summary>
        /// Get current ping
        /// </summary>
        /// <returns></returns>
        internal int GetPing()
        {
            return eunSocketObject.GetPing();
        }

        /// <summary>
        /// Get total send bytes from EUN Network to EUN Server
        /// </summary>
        /// <returns></returns>
        internal long GetTotalSendBytes()
        {
            return eunSocketObject.GetTotalSendBytes();
        }

        /// <summary>
        /// Get total receive bytes from EUN Server to EUN Network
        /// </summary>
        /// <returns></returns>
        internal long GetTotalRecvBytes()
        {
            return eunSocketObject.GetTotalRecvBytes();
        }

        /// <summary>
        /// Get total sends packets from EUN Network to EUN Server
        /// </summary>
        /// <returns></returns>
        internal long GetTotalSendPackets()
        {
            return eunSocketObject.GetTotalSendPackets();
        }

        /// <summary>
        /// Get total receive packets from EUN Server to EUN Network
        /// </summary>
        /// <returns></returns>
        internal long GetTotalRecvPackets()
        {
            return eunSocketObject.GetTotalRecvPackets();
        }
    }

    /// <summary>
    /// The networking peer statstics
    /// </summary>
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

        /// <summary>
        /// Get current ping
        /// </summary>
        /// <returns></returns>
        public int GetPing()
        {
            return peer.GetPing();
        }

        /// <summary>
        /// Get total send bytes from EUN Network to EUN Server
        /// </summary>
        /// <returns></returns>
        public long GetSendBytes()
        {
            return peer.GetTotalSendBytes() - lastRemoveSendBytes;
        }

        /// <summary>
        /// Get total receive bytes from EUN Server to EUN Network
        /// </summary>
        /// <returns></returns>
        public long GetRecvBytes()
        {
            return peer.GetTotalRecvBytes() - lastRemoveRecvBytes;
        }

        /// <summary>
        /// Clear all bytes send and receive
        /// </summary>
        public void ClearBytes()
        {
            lastRemoveSendBytes += peer.GetTotalSendBytes();
            lastRemoveRecvBytes += peer.GetTotalRecvBytes();
        }

        /// <summary>
        /// Get total sends packets from EUN Network to EUN Server
        /// </summary>
        /// <returns></returns>
        public long GetSendPackets()
        {
            return peer.GetTotalSendPackets() - lastRemoveSendPackets;
        }

        /// <summary>
        /// Get total receive packets from EUN Server to EUN Network
        /// </summary>
        /// <returns></returns>
        public long GetRecvPackets()
        {
            return peer.GetTotalRecvPackets() - lastRemoveRecvPackets;
        }

        /// <summary>
        /// Clear all packets send and receive
        /// </summary>
        public void ClearPackets()
        {
            lastRemoveSendPackets += peer.GetTotalSendPackets();
            lastRemoveRecvPackets += peer.GetTotalRecvPackets();
        }
    }
}
