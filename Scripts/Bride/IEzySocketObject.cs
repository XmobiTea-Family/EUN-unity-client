
namespace EUN.Bride
{
#if EUN
    using com.tvd12.ezyfoxserver.client.constant;
    using com.tvd12.ezyfoxserver.client.entity;
#else
    using EUN.Entity.Support;
#endif

    using System;
    using EUN.Common;

    public interface IEzySocketObject
    {
        void Init(string _zoneName, string _appName);
        void Connect(string username, string password, ICustomData data, string host, int port, int udpPort);
#if EUN
        void Send(EzyObject request, bool reliable = true);
#endif
        void SubscriberConnectionSuccessHandler(Action _onConnectionSuccess);
        void SubscriberConnectionFailureHandler(Action<EzyConnectionFailedReason> _onConnectionFailure);
        void SubscriberDisconnectionHandler(Action<EzyDisconnectReason> _onDisconnection);
        void SubscriberLoginErrorHandler(Action<CustomArray> _onLoginError);
        void SubscriberAppAccessHandler(Action<CustomArray> _onAppAccess);
        void SubscriberResponseHandler(Action<CustomArray> _onResponse);
        void SubscriberEventHandler(Action<CustomArray> _onEvent);

        int GetPing();

        long GetTotalSendBytes();
        long GetTotalRecvBytes();

        long GetTotalSendPackets();
        long GetTotalRecvPackets();
    }
}