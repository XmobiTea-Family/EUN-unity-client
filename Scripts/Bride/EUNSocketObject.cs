namespace XmobiTea.EUN.Bride
{
#if EUN_USING_ONLINE
    using com.tvd12.ezyfoxserver.client.entity;
    using com.tvd12.ezyfoxserver.client.constant;
    using com.tvd12.ezyfoxserver.client.logger;
#else
    using XmobiTea.EUN.Entity.Support;
#endif

    using XmobiTea.EUN.Logger;

    using System;
    using XmobiTea.EUN.Common;

    public abstract class EUNSocketObject : IEUNSocketObject
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

        public EUNSocketObject()
        {
#if EUN_DEBUG_EZYFOX
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
        public virtual void connect(string username, string password, IEUNData data, string host, int port, int udpPort)
        {
            EUNSocketObject.username = username;
            EUNSocketObject.password = password;
            EUNSocketObject.data = data;
        }

        /// <summary>
        /// Disconnect current connection ezyfox server (EUN Server)
        /// </summary>
        public virtual void disconnect() { }

        /// <summary>
        /// Init EUN Network
        /// </summary>
        /// <param name="_zoneName">the zone name in EUN Server settings</param>
        /// <param name="_appName">the app name and plugin name in EUN Server settings</param>
        public virtual void init(string _zoneName, string _appName)
        {
            EUNSocketObject.zoneName = _zoneName;
            EUNSocketObject.appName = _appName;
        }

        /// <summary>
        /// Subscriber an app access handler callback
        /// </summary>
        /// <param name="_onAppAccess"></param>
        public void subscriberAppAccessHandler(Action<EUNArray> _onAppAccess)
        {
            EUNSocketObject.onAppAccess = _onAppAccess;
        }

        /// <summary>
        /// Subscriber a connection failure handler callback
        /// </summary>
        /// <param name="_onConnectionFailure"></param>
        public void subscriberConnectionFailureHandler(Action<EzyConnectionFailedReason> _onConnectionFailure)
        {
            EUNSocketObject.onConnectionFailure = _onConnectionFailure;
        }

        /// <summary>
        /// Subscriber a connection success handler callback
        /// </summary>
        /// <param name="_onConnectionSuccess"></param>
        public void subscriberConnectionSuccessHandler(Action _onConnectionSuccess)
        {
            EUNSocketObject.onConnectionSuccess = _onConnectionSuccess;
        }

        /// <summary>
        /// Subscriber a disconnection handler callback
        /// </summary>
        /// <param name="_onDisconnection"></param>
        public void subscriberDisconnectionHandler(Action<EzyDisconnectReason> _onDisconnection)
        {
            EUNSocketObject.onDisconnection = _onDisconnection;
        }

        /// <summary>
        /// Subscriber a login error handler callback
        /// </summary>
        /// <param name="_onLoginError"></param>
        public void subscriberLoginErrorHandler(Action<EUNArray> _onLoginError)
        {
            EUNSocketObject.onLoginError = _onLoginError;
        }

        /// <summary>
        /// Subscriber event handler callback
        /// </summary>
        /// <param name="_onEvent"></param>
        public void subscriberEventHandler(Action<EUNArray> _onEvent)
        {
            EUNSocketObject.onEvent = _onEvent;
        }

        /// <summary>
        /// Subscriber response handler callback
        /// </summary>
        /// <param name="_onResponse"></param>
        public void subscriberResponseHandler(Action<EUNArray> _onResponse)
        {
            EUNSocketObject.onResponse = _onResponse;
        }

#if EUN_USING_ONLINE
        /// <summary>
        /// Send EzyObject request to ezyfox server
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reliable"></param>
        public virtual void send(EzyObject request, bool reliable = true)
        {

        }
#endif

        public virtual void service() { }

        /// <summary>
        /// Get current ping
        /// </summary>
        /// <returns></returns>
        public virtual int getPing()
        {
            return -500;
        }

        /// <summary>
        /// Get total bytes sent
        /// </summary>
        /// <returns></returns>
        public virtual long getTotalSendBytes()
        {
            return 0;
        }

        /// <summary>
        /// Get total bytes recv
        /// </summary>
        /// <returns></returns>
        public virtual long getTotalRecvBytes()
        {
            return 0;
        }

        /// <summary>
        /// Get total packets sent
        /// </summary>
        /// <returns></returns>
        public virtual long getTotalSendPackets()
        {
            return 0;
        }

        /// <summary>
        /// Get total packets recv
        /// </summary>
        /// <returns></returns>
        public virtual long getTotalRecvPackets()
        {
            return 0;
        }

    }

}
