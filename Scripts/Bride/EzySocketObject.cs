namespace EUN.Bride
{
    using UnityEngine;
#if EUN
    using com.tvd12.ezyfoxserver.client.entity;
    using com.tvd12.ezyfoxserver.client.constant;
    using com.tvd12.ezyfoxserver.client.logger;
#else
    using EUN.Entity.Support;
#endif

    using EUN.Logger;

    using System;
    using EUN.Common;

    public class EzySocketObject : MonoBehaviour, IEzySocketObject
    {
        internal static Action onConnectionSuccess;
        internal static Action<EzyConnectionFailedReason> onConnectionFailure;
        internal static Action<EzyDisconnectReason> onDisconnection;
        internal static Action<CustomArray> onAppAccess;
        internal static Action<CustomArray> onLoginError;
        internal static Action<CustomArray> onEvent;
        internal static Action<CustomArray> onResponse;

        internal static string zoneName;
        internal static string appName;
        internal static string username;
        internal static string password;
        internal static ICustomData data;

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

        public virtual void Connect(string username, string password, ICustomData data, string host, int port, int udpPort)
        {
            EzySocketObject.username = username;
            EzySocketObject.password = password;
            EzySocketObject.data = data;
        }

        public virtual void Init(string _zoneName, string _appName)
        {
            zoneName = _zoneName;
            appName = _appName;
        }

        public void SubscriberAppAccessHandler(Action<CustomArray> _onAppAccess)
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

        public void SubscriberLoginErrorHandler(Action<CustomArray> _onLoginError)
        {
            onLoginError = _onLoginError;
        }

        public void SubscriberEventHandler(Action<CustomArray> _onEvent)
        {
            onEvent = _onEvent;
        }

        public void SubscriberResponseHandler(Action<CustomArray> _onResponse)
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