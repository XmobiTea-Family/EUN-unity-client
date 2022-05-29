namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Entity;

    /// <summary>
    /// You can create custom IServerEventHandler by implement this handler
    /// </summary>
    public interface IServerEventHandler
    {
        /// <summary>
        /// The event code
        /// </summary>
        /// <returns></returns>
        int GetEventCode();

        /// <summary>
        /// This Handle automatic call if EUN Server call SendEvent to this client
        /// </summary>
        /// <param name="operationEvent"></param>
        /// <param name="peer"></param>
        void Handle(OperationEvent operationEvent, NetworkingPeer peer);
    }
}
