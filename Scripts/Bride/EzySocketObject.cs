namespace EUN.Bride
{
    using UnityEngine;

    using com.tvd12.ezyfoxserver.client.constant;
    using com.tvd12.ezyfoxserver.client.entity;
    using com.tvd12.ezyfoxserver.client.logger;

    using EUN.Logger;

    using System;

    public class EzySocketObject : MonoBehaviour, IEzySocketObject
    {
        internal static Action onConnectionSuccess;
        internal static Action<EzyConnectionFailedReason> onConnectionFailure;
        internal static Action<EzyDisconnectReason> onDisconnection;
        internal static Action<EzyArray> onAppAccess;
        internal static Action<EzyArray> onLoginError;
        internal static Action<EzyArray> onEvent;
        internal static Action<EzyArray> onResponse;

        internal static string zoneName;
        internal static string appName;
        internal static string username;
        internal static string password;
        internal static EzyData data;


        void Start()
        {
            OnCustomStart();
        }

        protected virtual void OnCustomStart()
        {
            EzyLoggerFactory.setLoggerSupply(type => new UnityLogger(type));
        }

        public virtual void Connect(string username, string password, EzyData data, string host, int port, int udpPort)
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

        public void SubscriberAppAccessHandler(Action<EzyArray> _onAppAccess)
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

        public void SubscriberLoginErrorHandler(Action<EzyArray> _onLoginError)
        {
            onLoginError = _onLoginError;
        }

        public void SubscriberEventHandler(Action<EzyArray> _onEvent)
        {
            onEvent = _onEvent;
        }

        public void SubscriberResponseHandler(Action<EzyArray> _onResponse)
        {
            onResponse = _onResponse;
        }

        public virtual void Send(EzyObject request, bool reliable = true) { }
    }
}