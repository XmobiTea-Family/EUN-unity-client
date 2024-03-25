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
            return eunSocketObject.getPing();
        }

        /// <summary>
        /// Get total send bytes from EUN Network to EUN Server
        /// </summary>
        /// <returns></returns>
        internal long GetTotalSendBytes()
        {
            return eunSocketObject.getTotalSendBytes();
        }

        /// <summary>
        /// Get total receive bytes from EUN Server to EUN Network
        /// </summary>
        /// <returns></returns>
        internal long GetTotalRecvBytes()
        {
            return eunSocketObject.getTotalRecvBytes();
        }

        /// <summary>
        /// Get total sends packets from EUN Network to EUN Server
        /// </summary>
        /// <returns></returns>
        internal long GetTotalSendPackets()
        {
            return eunSocketObject.getTotalSendPackets();
        }

        /// <summary>
        /// Get total receive packets from EUN Server to EUN Network
        /// </summary>
        /// <returns></returns>
        internal long GetTotalRecvPackets()
        {
            return eunSocketObject.getTotalRecvPackets();
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
        public int getPing()
        {
            return this.peer.GetPing();
        }

        /// <summary>
        /// Get total send bytes from EUN Network to EUN Server
        /// </summary>
        /// <returns></returns>
        public long getSendBytes()
        {
            return this.peer.GetTotalSendBytes() - this.lastRemoveSendBytes;
        }

        /// <summary>
        /// Get total receive bytes from EUN Server to EUN Network
        /// </summary>
        /// <returns></returns>
        public long getRecvBytes()
        {
            return this.peer.GetTotalRecvBytes() - this.lastRemoveRecvBytes;
        }

        /// <summary>
        /// Clear all bytes send and receive
        /// </summary>
        public void clearBytes()
        {
            this.lastRemoveSendBytes += this.peer.GetTotalSendBytes();
            this.lastRemoveRecvBytes += this.peer.GetTotalRecvBytes();
        }

        /// <summary>
        /// Get total sends packets from EUN Network to EUN Server
        /// </summary>
        /// <returns></returns>
        public long getSendPackets()
        {
            return this.peer.GetTotalSendPackets() - this.lastRemoveSendPackets;
        }

        /// <summary>
        /// Get total receive packets from EUN Server to EUN Network
        /// </summary>
        /// <returns></returns>
        public long getRecvPackets()
        {
            return this.peer.GetTotalRecvPackets() - this.lastRemoveRecvPackets;
        }

        /// <summary>
        /// Clear all packets send and receive
        /// </summary>
        public void clearPackets()
        {
            this.lastRemoveSendPackets += this.peer.GetTotalSendPackets();
            this.lastRemoveRecvPackets += this.peer.GetTotalRecvPackets();
        }

    }

}
