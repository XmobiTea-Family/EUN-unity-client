namespace EUN
{
    using EUN.Entity;
    using EUN.Extension;

    using System.Collections.Generic;

    using UnityEngine;

    [DisallowMultipleComponent]
    public sealed class EzyView : MonoBehaviour
    {
        [SerializeField]
        private RoomGameObject roomObjectGame;
        public RoomGameObject RoomGameObject
        {
            get { return roomObjectGame; }
            private set
            {
                roomObjectGame = value;
            }
        }

        internal List<EzyBehaviour> ezyBehaviourLst { get; private set; } = new List<EzyBehaviour>();

        internal List<EzyVoiceChatBehaviour> ezyVoiceChatBehaviourLst { get; private set; } = new List<EzyVoiceChatBehaviour>();

        public RoomPlayer Owner => EzyNetwork.GetRoomPlayer(RoomGameObject.OwnerId);

        public bool IsMine => RoomGameObject != null && RoomGameObject.OwnerId == EzyNetwork.PlayerId;

        private void Awake()
        {
            EzyNetwork.SubscriberEzyView(this);
        }

        private void OnDestroy()
        {
            EzyNetwork.UnSubscriberEzyView(this);
        }

        internal void Init(RoomGameObject roomGameObject)
        {
            this.RoomGameObject = roomGameObject;
        }

        internal void SubscriberEzyBehaviour(EzyBehaviour behaviour)
        {
            if (!ezyBehaviourLst.Contains(behaviour))
            {
                ezyBehaviourLst.Add(behaviour);

                behaviour.OnEzyInitialize(RoomGameObject.InitializeData);
                behaviour.OnEzySynchronization(RoomGameObject.SynchronizationData);
            }
        }

        internal void SubscriberEzyBehaviour(EzyVoiceChatBehaviour behaviour)
        {
            if (!ezyVoiceChatBehaviourLst.Contains(behaviour))
            {
                ezyVoiceChatBehaviourLst.Add(behaviour);
            }
        }

        internal void UnSubscriberEzyBehaviour(EzyBehaviour behaviour)
        {
            if (ezyBehaviourLst.Contains(behaviour)) ezyBehaviourLst.Remove(behaviour);
        }

        internal void UnSubscriberEzyBehaviour(EzyVoiceChatBehaviour behaviour)
        {
            if (ezyVoiceChatBehaviourLst.Contains(behaviour)) ezyVoiceChatBehaviourLst.Remove(behaviour);
        }

        public void RPC(EzyTargets targets, EzyRPCCommand command, params object[] rpcData)
        {
            EzyNetworkExtension.RPC(this, targets, command, rpcData);
        }
    }
}