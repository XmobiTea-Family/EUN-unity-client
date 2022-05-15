namespace XmobiTea.EUN
{
    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Extension;

    using System.Collections.Generic;

    using UnityEngine;
    using XmobiTea.EUN.Constant;

    [DisallowMultipleComponent]
    public sealed class EUNView : MonoBehaviour
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

        internal List<EUNBehaviour> eunBehaviourLst { get; private set; } = new List<EUNBehaviour>();

        internal List<EUNVoiceChatBehaviour> eunVoiceChatBehaviourLst { get; private set; } = new List<EUNVoiceChatBehaviour>();

        public RoomPlayer Owner => EUNNetwork.GetRoomPlayer(RoomGameObject.OwnerId);

        public bool IsMine => RoomGameObject != null && RoomGameObject.OwnerId == EUNNetwork.PlayerId;

        private void Awake()
        {
            EUNNetwork.SubscriberEUNView(this);
        }

        private void OnDestroy()
        {
            EUNNetwork.UnSubscriberEUNView(this);
        }

        internal void Init(RoomGameObject roomGameObject)
        {
            this.RoomGameObject = roomGameObject;
        }

        internal void SubscriberEUNBehaviour(EUNBehaviour behaviour)
        {
            if (!eunBehaviourLst.Contains(behaviour))
            {
                eunBehaviourLst.Add(behaviour);

                behaviour.OnEUNInitialize(RoomGameObject.InitializeData);
                behaviour.OnEUNSynchronization(RoomGameObject.SynchronizationData);
            }
        }

        internal void SubscriberEUNBehaviour(EUNVoiceChatBehaviour behaviour)
        {
            if (!eunVoiceChatBehaviourLst.Contains(behaviour))
            {
                eunVoiceChatBehaviourLst.Add(behaviour);
            }
        }

        internal void UnSubscriberEUNBehaviour(EUNBehaviour behaviour)
        {
            if (eunBehaviourLst.Contains(behaviour)) eunBehaviourLst.Remove(behaviour);
        }

        internal void UnSubscriberEUNBehaviour(EUNVoiceChatBehaviour behaviour)
        {
            if (eunVoiceChatBehaviourLst.Contains(behaviour)) eunVoiceChatBehaviourLst.Remove(behaviour);
        }

        public void RPC(EUNTargets targets, EUNRPCCommand command, params object[] rpcData)
        {
            EUNNetworkExtension.RPC(this, targets, command, rpcData);
        }
    }
}
