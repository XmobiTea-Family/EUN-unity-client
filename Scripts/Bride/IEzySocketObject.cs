namespace EUN.Bride
{
    using com.tvd12.ezyfoxserver.client.constant;
    using com.tvd12.ezyfoxserver.client.entity;

    using System;

    using static EUN.Config.EzyServerSettings;

    public interface IEzySocketObject
    {
        void Init(string _zoneName, string _appName);
        void Connect(string userId, string host, int port, int udpPort);
        void Send(EzyObject request, bool reliable = true);
        void SubscriberConnectionSuccessHandler(Action _onConnectionSuccess);
        void SubscriberConnectionFailureHandler(Action<EzyConnectionFailedReason> _onConnectionFailure);
        void SubscriberDisconnectionHandler(Action<EzyDisconnectReason> _onDisconnection);
        void SubscriberLoginErrorHandler(Action<EzyArray> _onLoginError);
        void SubscriberAppAccessHandler(Action<EzyArray> _onAppAccess);
        void SubscriberResponseHandler(Action<EzyArray> _onResponse);
        void SubscriberEventHandler(Action<EzyArray> _onEvent);
    }
}