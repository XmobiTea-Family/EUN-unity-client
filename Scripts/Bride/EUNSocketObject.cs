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

        /// <summary>
        /// Connect to ezyfox server (EUN Server)
        /// </summary>
        /// <param name="username">for EUN Server, username is userid</param>
        /// <param name="password"></param>
        /// <param name="data"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="udpPort"></param>
        public virtual void Connect(string username, string password, IEUNData data, string host, int port, int udpPort)
        {
            EUNSocketObject.username = username;
            EUNSocketObject.password = password;
            EUNSocketObject.data = data;
        }

        public virtual void Disconnect() { }

        /// <summary>
        /// Init EUN Network
        /// </summary>
        /// <param name="_zoneName">the zone name in EUN Server settings</param>
        /// <param name="_appName">the app name and plugin name in EUN Server settings</param>
        public virtual void Init(string _zoneName, string _appName)
        {
            zoneName = _zoneName;
            appName = _appName;
        }

        /// <summary>
        /// Subscriber an app access handler callback
        /// </summary>
        /// <param name="_onAppAccess"></param>
        public void SubscriberAppAccessHandler(Action<EUNArray> _onAppAccess)
        {
            onAppAccess = _onAppAccess;
        }

        /// <summary>
        /// Subscriber a connection failure handler callback
        /// </summary>
        /// <param name="_onConnectionFailure"></param>
        public void SubscriberConnectionFailureHandler(Action<EzyConnectionFailedReason> _onConnectionFailure)
        {
            onConnectionFailure = _onConnectionFailure;
        }

        /// <summary>
        /// Subscriber a connection success handler callback
        /// </summary>
        /// <param name="_onConnectionSuccess"></param>
        public void SubscriberConnectionSuccessHandler(Action _onConnectionSuccess)
        {
            onConnectionSuccess = _onConnectionSuccess;
        }

        /// <summary>
        /// Subscriber a disconnection handler callback
        /// </summary>
        /// <param name="_onDisconnection"></param>
        public void SubscriberDisconnectionHandler(Action<EzyDisconnectReason> _onDisconnection)
        {
            onDisconnection = _onDisconnection;
        }

        /// <summary>
        /// Subscriber a login error handler callback
        /// </summary>
        /// <param name="_onLoginError"></param>
        public void SubscriberLoginErrorHandler(Action<EUNArray> _onLoginError)
        {
            onLoginError = _onLoginError;
        }

        /// <summary>
        /// Subscriber event handler callback
        /// </summary>
        /// <param name="_onEvent"></param>
        public void SubscriberEventHandler(Action<EUNArray> _onEvent)
        {
            onEvent = _onEvent;
        }

        /// <summary>
        /// Subscriber response handler callback
        /// </summary>
        /// <param name="_onResponse"></param>
        public void SubscriberResponseHandler(Action<EUNArray> _onResponse)
        {
            onResponse = _onResponse;
        }

#if EUN
        public virtual void Send(EzyObject request, bool reliable = true)
        {

        }
#endif

        /// <summary>
        /// Get current ping
        /// </summary>
        /// <returns></returns>
        public virtual int GetPing()
        {
            return -500;
        }

        /// <summary>
        /// Get total bytes sent
        /// </summary>
        /// <returns></returns>
        public virtual long GetTotalSendBytes()
        {
            return 0;
        }

        /// <summary>
        /// Get total bytes recv
        /// </summary>
        /// <returns></returns>
        public virtual long GetTotalRecvBytes()
        {
            return 0;
        }

        /// <summary>
        /// Get total packets sent
        /// </summary>
        /// <returns></returns>
        public virtual long GetTotalSendPackets()
        {
            return 0;
        }

        /// <summary>
        /// Get total packets recv
        /// </summary>
        /// <returns></returns>
        public virtual long GetTotalRecvPackets()
        {
            return 0;
        }
    }
}
