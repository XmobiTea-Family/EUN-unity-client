namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    internal interface IServerEventHandler
    {
        EventCode GetEventCode();
        void Handle(OperationEvent operationEvent, NetworkingPeer peer);
    }
}