namespace XmobiTea.EUN.Bride
{
#if EUN_USING_ONLINE
    using com.tvd12.ezyfoxserver.client.constant;
    using com.tvd12.ezyfoxserver.client.entity;
#else
    using XmobiTea.EUN.Entity.Support;
#endif

    using System;
    using XmobiTea.EUN.Common;

    public interface IEUNSocketObject
    {
        void init(string _zoneName, string _appName);
        void connect(string username, string password, IEUNData data, string host, int port, int udpPort);
        void disconnect();
#if EUN_USING_ONLINE
        void send(EzyObject request, bool reliable = true);
#endif
        void subscriberConnectionSuccessHandler(Action _onConnectionSuccess);
        void subscriberConnectionFailureHandler(Action<EzyConnectionFailedReason> _onConnectionFailure);
        void subscriberDisconnectionHandler(Action<EzyDisconnectReason> _onDisconnection);
        void subscriberLoginErrorHandler(Action<EUNArray> _onLoginError);
        void subscriberAppAccessHandler(Action<EUNArray> _onAppAccess);
        void subscriberResponseHandler(Action<EUNArray> _onResponse);
        void subscriberEventHandler(Action<EUNArray> _onEvent);

        void service();

        int getPing();

        long getTotalSendBytes();
        long getTotalRecvBytes();

        long getTotalSendPackets();
        long getTotalRecvPackets();

    }

}
