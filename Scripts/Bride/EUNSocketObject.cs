namespace XmobiTea.EUN.Bride
{
    using UnityEngine;
#if EUN
    using com.tvd12.ezyfoxserver.client.entity;
    using com.tvd12.ezyfoxserver.client.constant;
    using com.tvd12.ezyfoxserver.client.logger;
#else
    using XmobiTea.EUN.Entity.Support;
#endif

    using XmobiTea.EUN.Logger;

    using System;
    using XmobiTea.EUN.Common;

    public class EUNSocketObject : MonoBehaviour, IEUNSocketObject
    {
        internal static Action onConnectionSuccess;
        internal static Action<EzyConnectionFailedReason> onConnectionFailure;
        internal static Action<EzyDisconnectReason> onDisconnection;
        internal static Action<EUNArray> onAppAccess;
        internal static Action<EUNArray> onLoginError;
        internal static Action<EUNArray> onEvent;
        internal static Action<EUNArray> onResponse;

        internal static string zoneName;
        internal static string appName;
        internal static string username;
        internal static string password;
        internal static IEUNData data;

        void Start()
        {
            OnCustomStart();
        }

        protected virtual void OnCustomStart()
        {
#if EUN
            EzyLoggerFactory.setLoggerSupply(type => new UnityLogger(type));
#endif
        }

        public virtual void Connect(string username, string password, IEUNData data, string host, int port, int udpPort)
        {
            EUNSocketObject.username = username;
            EUNSocketObject.password = password;
            EUNSocketObject.data = data;
        }

        public virtual void Disconnect() { }

        public virtual void Init(string _zoneName, string _appName)
        {
            zoneName = _zoneName;
            appName = _appName;
        }

        public void SubscriberAppAccessHandler(Action<EUNArray> _onAppAccess)
        {
            onAppAccess = _onAppAccess;
        }

        public void SubscriberConnectionFailureHandler(Action<EzyConnectionFailedReason> _onConnectionFailure)
        {
            onConnectionFailure = _onConnectionFailure;
        }

        public void SubscriberConnectionSuccessHandler(Action _onConnectionSuccess)
        {
            onConnectionSuccess = _onConnectionSuccess;
        }

        public void SubscriberDisconnectionHandler(Action<EzyDisconnectReason> _onDisconnection)
        {
            onDisconnection = _onDisconnection;
        }

        public void SubscriberLoginErrorHandler(Action<EUNArray> _onLoginError)
        {
            onLoginError = _onLoginError;
        }

        public void SubscriberEventHandler(Action<EUNArray> _onEvent)
        {
            onEvent = _onEvent;
        }

        public void SubscriberResponseHandler(Action<EUNArray> _onResponse)
        {
            onResponse = _onResponse;
        }

#if EUN
        public virtual void Send(EzyObject request, bool reliable = true)
        {

        }
#endif

        public virtual int GetPing()
        {
            return -500;
        }

        public virtual long GetTotalSendBytes()
        {
            return 0;
        }

        public virtual long GetTotalRecvBytes()
        {
            return 0;
        }

        public virtual long GetTotalSendPackets()
        {
            return 0;
        }

        public virtual long GetTotalRecvPackets()
        {
            return 0;
        }
    }
}
