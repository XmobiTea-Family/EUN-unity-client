namespace XmobiTea.EUN
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Networking;
    using XmobiTea.EUN.Config;

    using System;

    using UnityEngine;
    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Logger;

    public static partial class EUNNetwork
    {
        /// <summary>
        /// The current version of EUN client
        /// </summary>
        public const string Version = "1.2.2";

        /// <summary>
        /// The mode for connection EUN client
        /// </summary>
        public static EUNServerSettings.Mode Mode => eunServerSettings != null ? eunServerSettings.mode : EUNServerSettings.Mode.OfflineMode;

        /// <summary>
        /// The current user id client has authenticated
        /// </summary>
        public static string UserId { get; private set; }

        /// <summary>
        /// The current ServerSettings
        /// It store in path EUN-unity-client-custom/Resources/EUNServerSettings.asset
        /// </summary>
        public static EUNServerSettings eunServerSettings { get; private set; }

        /// <summary>
        /// The current peer of EUN client
        /// </summary>
        private static NetworkingPeer peer;

        /// <summary>
        /// The current peer statistics
        /// </summary>
        private static NetworkingPeerStatistics peerStatistics;

        static EUNNetwork()
        {
            InitServerSettings();
            InitEUNDebug();
            
            if (!Application.isPlaying) return;

            InitEUNSocketObject();
            InitEUNSocketStatisticsObject();
        }

        /// <summary>
        /// Init server settings, load the EUNServerSettings in Resources folder
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        private static void InitServerSettings()
        {
            eunServerSettings = Resources.Load(EUNServerSettings.ResourcesPath) as EUNServerSettings;

            if (eunServerSettings == null)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.ExecuteMenuItem("EUN/EUN Settings");
                eunServerSettings = Resources.Load(EUNServerSettings.ResourcesPath) as EUNServerSettings;
                if (eunServerSettings == null) throw new NullReferenceException("Null EUN Server Settings, please find it now");
#endif
            }
        }

        /// <summary>
        /// Init Debug, settings for debug in the EUNServerSettings LogType
        /// </summary>
        /// <exception cref="NullReferenceException">EUNServerSettings not found</exception>
        private static void InitEUNDebug()
        {
            if (eunServerSettings == null) throw new NullReferenceException("Null EUN Server Settings, please find it now");

            EUNDebug.logType = eunServerSettings.logType;
        }

        /// <summary>
        /// Init the socket object for EUN client
        /// </summary>
        private static void InitEUNSocketObject()
        {
            peer = new GameObject("EUN NetworkingPeer").AddComponent<NetworkingPeer>();
            GameObject.DontDestroyOnLoad(peer.gameObject);

            peer.InitPeer();
        }

        /// <summary>
        /// Init the socket statistics object for EUN client
        /// </summary>
        private static void InitEUNSocketStatisticsObject()
        {
            peerStatistics = new NetworkingPeerStatistics(peer);
        }

        /// <summary>
        /// Connect client to EUN Server with user id and custom data
        /// It received at UserLoginController EUN-plugin in EUN Server
        /// </summary>
        /// <param name="userId">The user id you want authenticated to server</param>
        /// <param name="customData">The custom data need send to EUN Server, like auth token, username or password</param>
        public static void Connect(string userId, EUNArray customData = null)
        {
            UserId = userId;

            peer.Connect(UserId, string.Empty, customData);
        }

        /// <summary>
        /// Disconnect client
        /// </summary>
        public static void Disconnect()
        {
            peer.Disconnect();
        }

        /// <summary>
        /// Send a request to EUN Server
        /// You can send request if EUNNetwork.IsConnected == true
        /// </summary>
        /// <param name="request"></param>
        /// <param name="onResponse"></param>
        public static void Send(OperationRequest request, Action<OperationResponse> onResponse = null)
        {
            peer.Enqueue(request, onResponse);
        }

        /// <summary>
        /// Get network statistics to get packet, bytes sent or received, success or not 
        /// </summary>
        /// <returns></returns>
        public static NetworkingPeerStatistics GetPeerStatistics() { return peerStatistics; }

        /// <summary>
        /// Subscriber a IEUNManagerBehaviour
        /// </summary>
        /// <param name="behaviour">what a IEUNManagerBehaviour</param>
        public static void SubscriberEUNManagerBehaviour(IEUNManagerBehaviour behaviour)
        {
            peer.SubscriberEUNManagerBehaviour(behaviour);
        }

        /// <summary>
        /// Unsubscriber a IEUNManagerBehaviour
        /// </summary>
        /// <param name="behaviour">what a IEUNManagerBehaviour</param>
        public static void UnSubscriberEUNManagerBehaviour(IEUNManagerBehaviour behaviour)
        {
            peer.UnSubscriberEUNManagerBehaviour(behaviour);
        }

        /// <summary>
        /// Subscriber a EUNView
        /// </summary>
        /// <param name="view"></param>
        internal static void SubscriberEUNView(EUNView view)
        {
            peer.SubscriberEUNView(view);
        }

        /// <summary>
        /// Unsubscriber a EUNView
        /// </summary>
        /// <param name="view"></param>
        internal static void UnSubscriberEUNView(EUNView view)
        {
            peer.UnSubscriberEUNView(view);
        }
    }
}
