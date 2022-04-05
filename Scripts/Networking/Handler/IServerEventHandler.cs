namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;

    internal interface IServerEventHandler
    {
        EventCode GetEventCode();
        void Handle(OperationEvent operationEvent, NetworkingPeer peer);
    }
}