namespace EUN
{
#if EUN
    using com.tvd12.ezyfoxserver.client.entity;
#endif
    using EUN.Common;
    using EUN.Networking;
    using EUN.Config;

    using System;

    using UnityEngine;

    using System.Collections.Generic;

    public static partial class EzyNetwork
    {
        public const string Version = "1.0.0";
        public static EzyServerSettings.Mode Mode => ezyServerSettings != null ? ezyServerSettings.mode : EzyServerSettings.Mode.OfflineMode;

        public static string UserId { get; private set; }

        public static EzyServerSettings ezyServerSettings { get; private set; }

        private static NetworkingPeer peer;

        static EzyNetwork()
        {
            InitEzySocketObject();
        }

        private static void InitEzySocketObject()
        {
#if EUN
            ezyServerSettings = Resources.Load(EzyServerSettings.ResourcesPath) as EzyServerSettings;

            peer = new GameObject("EUN NetworkingPeer").AddComponent<NetworkingPeer>();
            GameObject.DontDestroyOnLoad(peer.gameObject);

            peer.InitPeer();
#endif
        }

        public static void Connect(string userId, IList<object> data)
        {
#if EUN
            UserId = userId;

            peer.Connect(UserId, string.Empty, data);
#endif
        }

        public static void Send(OperationRequest request, Action<OperationResponse> onResponse = null)
        {
#if EUN
            peer.Enqueue(request, onResponse);
#endif
        }

        internal static void SubscriberEzyBehaviour(EzyManagerBehaviour behaviour)
        {
#if EUN
            peer.SubscriberEzyBehaviour(behaviour);
#endif
        }

        internal static void UnSubscriberEzyBehaviour(EzyManagerBehaviour behaviour)
        {
#if EUN
            peer.UnSubscriberEzyBehaviour(behaviour);
#endif
        }

        internal static void SubscriberEzyView(EzyView view)
        {
#if EUN
            peer.SubscriberEzyView(view);
#endif
        }

        internal static void UnSubscriberEzyView(EzyView view)
        {
#if EUN
            peer.UnSubscriberEzyView(view);
#endif
        }
    }
}