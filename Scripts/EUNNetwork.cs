namespace XmobiTea.EUN
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Networking;
    using XmobiTea.EUN.Config;

    using System;

    using UnityEngine;
    using XmobiTea.EUN.Entity;

    public static partial class EUNNetwork
    {
        public const string Version = "1.0.0";
        public static EUNServerSettings.Mode Mode => eunServerSettings != null ? eunServerSettings.mode : EUNServerSettings.Mode.OfflineMode;

        public static string UserId { get; private set; }

        public static EUNServerSettings eunServerSettings { get; private set; }

        private static NetworkingPeer peer;

        private static NetworkingPeerStatistics peerStatistics;

        static EUNNetwork()
        {
            InitServerSettings();

            if (!Application.isPlaying) return;

            InitEUNSocketObject();
            InitEUNSocketStatisticsObject();
        }

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

        private static void InitEUNSocketObject()
        {
            peer = new GameObject("EUN NetworkingPeer").AddComponent<NetworkingPeer>();
            GameObject.DontDestroyOnLoad(peer.gameObject);

            peer.InitPeer();
        }

        private static void InitEUNSocketStatisticsObject()
        {
            peerStatistics = new NetworkingPeerStatistics(peer);
        }

        public static void Connect(string userId, EUNArray data)
        {
            UserId = userId;

            peer.Connect(UserId, string.Empty, data);
        }

        public static void Send(OperationRequest request, Action<OperationResponse> onResponse = null)
        {
            peer.Enqueue(request, onResponse);
        }

        public static NetworkingPeerStatistics GetPeerStatistics() { return peerStatistics; }

        internal static void SubscriberEUNBehaviour(EUNManagerBehaviour behaviour)
        {
            peer.SubscriberEUNBehaviour(behaviour);
        }

        internal static void UnSubscriberEUNBehaviour(EUNManagerBehaviour behaviour)
        {
            peer.UnSubscriberEUNBehaviour(behaviour);
        }

        internal static void SubscriberEUNView(EUNView view)
        {
            peer.SubscriberEUNView(view);
        }

        internal static void UnSubscriberEUNView(EUNView view)
        {
            peer.UnSubscriberEUNView(view);
        }
    }
}