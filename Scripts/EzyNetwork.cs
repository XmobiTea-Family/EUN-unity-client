namespace EUN
{
    using com.tvd12.ezyfoxserver.client.entity;
    using EUN.Common;
    using EUN.Config;
    using EUN.Networking;

    using System;

    using UnityEngine;

    public static partial class EzyNetwork
    {
        public static string UserId { get; private set; }

        public static EzyServerSettings ezyServerSettings { get; private set; }

        private static NetworkingPeer peer;

        static EzyNetwork()
        {
            InitEzySocketObject();
        }

        private static void InitEzySocketObject()
        {
            ezyServerSettings = Resources.Load(EzyServerSettings.ResourcesPath) as EzyServerSettings;

            peer = new GameObject("EUN NetworkingPeer").AddComponent<NetworkingPeer>();
            GameObject.DontDestroyOnLoad(peer.gameObject);

            peer.InitPeer();
        }

        public static void Connect(string userId, EzyData data)
        {
            UserId = userId;

            peer.Connect(UserId, string.Empty, data);
        }

        public static void Send(OperationRequest request, Action<OperationResponse> onResponse = null)
        {
            peer.Enqueue(request, onResponse);
        }

        //public static void SubscriberEzyBehaviour(EzyBehaviour behaviour)
        //{
        //    peer.SubscriberEzyBehaviour(behaviour);
        //}

        //public static void UnSubscriberEzyBehaviour(EzyBehaviour behaviour)
        //{
        //    peer.UnSubscriberEzyBehaviour(behaviour);
        //}

        internal static void SubscriberEzyBehaviour(EzyManagerBehaviour behaviour)
        {
            peer.SubscriberEzyBehaviour(behaviour);
        }

        internal static void UnSubscriberEzyBehaviour(EzyManagerBehaviour behaviour)
        {
            peer.UnSubscriberEzyBehaviour(behaviour);
        }

        internal static void SubscriberEzyView(EzyView view)
        {
            peer.SubscriberEzyView(view);
        }

        internal static void UnSubscriberEzyView(EzyView view)
        {
            peer.UnSubscriberEzyView(view);
        }
    }
}