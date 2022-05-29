namespace XmobiTea.EUN.Bride
{
#if EUN
    using com.tvd12.ezyfoxserver.client.constant;
    using com.tvd12.ezyfoxserver.client.entity;
#else
    using XmobiTea.EUN.Entity.Support;
#endif

    using System;
    using XmobiTea.EUN.Common;

    public interface IEUNSocketObject
    {
        void Init(string _zoneName, string _appName);
        void Connect(string username, string password, IEUNData data, string host, int port, int udpPort);
        void Disconnect();
#if EUN
        void Send(EzyObject request, bool reliable = true);
#endif
        void SubscriberConnectionSuccessHandler(Action _onConnectionSuccess);
        void SubscriberConnectionFailureHandler(Action<EzyConnectionFailedReason> _onConnectionFailure);
        void SubscriberDisconnectionHandler(Action<EzyDisconnectReason> _onDisconnection);
        void SubscriberLoginErrorHandler(Action<EUNArray> _onLoginError);
        void SubscriberAppAccessHandler(Action<EUNArray> _onAppAccess);
        void SubscriberResponseHandler(Action<EUNArray> _onResponse);
        void SubscriberEventHandler(Action<EUNArray> _onEvent);

        int GetPing();

        long GetTotalSendBytes();
        long GetTotalRecvBytes();

        long GetTotalSendPackets();
        long GetTotalRecvPackets();
    }
}
