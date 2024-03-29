﻿namespace XmobiTea.EUN
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Networking;
    using XmobiTea.EUN.Config;

    using System;

    using UnityEngine;
    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Logger;
    using System.Threading.Tasks;
    using XmobiTea.EUN.Unity;

    public static partial class EUNNetwork
    {
        /// <summary>
        /// The current version of EUN client
        /// </summary>
        private const string EUN_VERSION = "1.2.2";

        /// <summary>
        /// The mode for connection EUN client
        /// </summary>
        public static EUNServerSettings.Mode mode => eunServerSettings != null ? eunServerSettings.mode : EUNServerSettings.Mode.OfflineMode;

        /// <summary>
        /// The current user id client has authenticated
        /// </summary>
        public static string userId { get; private set; }

        /// <summary>
        /// The current ServerSettings
        /// It store in path EUN-unity-client-custom/Resources/EUNServerSettings.asset
        /// </summary>
        public static EUNServerSettings eunServerSettings { get; private set; }

        /// <summary>
        /// The current peer of EUN client
        /// </summary>
        internal static NetworkingPeer peer { get; private set; }

        /// <summary>
        /// The current peer statistics
        /// </summary>
        private static NetworkingPeerStatistics peerStatistics;

        static EUNNetwork()
        {
            initServerSettings();
            initEUNDebug();
            
            if (!Application.isPlaying) return;

            initEUNSocketObject();
            initEUNSocketStatisticsObject();
        }

        /// <summary>
        /// Get the Client Sdk Version
        /// </summary>
        /// <returns></returns>
        public static string getClientSdkVersion() => EUNNetwork.EUN_VERSION;

        /// <summary>
        /// Init server settings, load the EUNServerSettings in Resources folder
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        private static void initServerSettings()
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
        private static void initEUNDebug()
        {
            if (eunServerSettings == null) throw new NullReferenceException("Null EUN Server Settings, please find it now");

            EUNDebug.init(eunServerSettings.logType);
        }

        /// <summary>
        /// Init the socket object for EUN client
        /// </summary>
        private static void initEUNSocketObject()
        {
            var peer = new NetworkingPeer();
            peer.initPeer();

            EUNNetwork.peer = peer;

            var serviceUpdate = new GameObject("[EUN] ServiceUpdate").AddComponent<ServiceUpdate>();
            GameObject.DontDestroyOnLoad(serviceUpdate.gameObject);

            serviceUpdate.peer = peer;
        }

        /// <summary>
        /// Init the socket statistics object for EUN client
        /// </summary>
        private static void initEUNSocketStatisticsObject()
        {
            peerStatistics = new NetworkingPeerStatistics(peer);
        }

        /// <summary>
        /// Connect client to EUN Server with user id and custom data
        /// It received at UserLoginController EUN-plugin in EUN Server
        /// </summary>
        /// <param name="userId">The user id you want authenticated to server</param>
        /// <param name="customData">The custom data need send to EUN Server, like auth token, username or password</param>
        public static void connect(string userId, EUNArray customData = null)
        {
            if (eunServerSettings == null) throw new NullReferenceException("Null EUN Server Settings, please find it now");

            EUNNetwork.userId = userId;

            peer.connect(EUNNetwork.userId, eunServerSettings.secretKey, customData);
        }

        /// <summary>
        /// Disconnect client
        /// </summary>
        public static void disconnect()
        {
            peer.disconnect();
        }

        /// <summary>
        /// Send a request to EUN Server
        /// You can send request if EUNNetwork.IsConnected == true
        /// </summary>
        /// <param name="request"></param>
        /// <param name="onResponse"></param>
        public static void send(OperationRequest request, Action<OperationResponse> onResponse = null)
        {
            peer.enqueue(request, onResponse);
        }

        public static async Task<OperationResponse> sendAsync(OperationRequest request)
        {
            OperationResponse waitingResult = null;

            send(request, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Get network statistics to get packet, bytes sent or received, success or not 
        /// </summary>
        /// <returns></returns>
        public static NetworkingPeerStatistics getPeerStatistics() { return peerStatistics; }

        /// <summary>
        /// Subscriber a IEUNManagerBehaviour
        /// </summary>
        /// <param name="behaviour">what a IEUNManagerBehaviour</param>
        public static void subscriberEUNManagerBehaviour(IEUNManagerBehaviour behaviour)
        {
            peer.subscriberEUNManagerBehaviour(behaviour);
        }

        /// <summary>
        /// Unsubscriber a IEUNManagerBehaviour
        /// </summary>
        /// <param name="behaviour">what a IEUNManagerBehaviour</param>
        public static void unSubscriberEUNManagerBehaviour(IEUNManagerBehaviour behaviour)
        {
            peer.unSubscriberEUNManagerBehaviour(behaviour);
        }

        /// <summary>
        /// Subscriber a EUNView
        /// </summary>
        /// <param name="view"></param>
        internal static void subscriberEUNView(EUNView view)
        {
            peer.subscriberEUNView(view);
        }

        /// <summary>
        /// Unsubscriber a EUNView
        /// </summary>
        /// <param name="view"></param>
        internal static void unSubscriberEUNView(EUNView view)
        {
            peer.unSubscriberEUNView(view);
        }

    }

}
