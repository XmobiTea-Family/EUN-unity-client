namespace EUN.Networking
{
    using EUN.Common;
    using EUN.Constant;

    internal interface IServerEventHandler
    {
        EventCode GetEventCode();
        void Handle(OperationEvent operationEvent, NetworkingPeer peer);
    }
}