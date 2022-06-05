namespace XmobiTea.EUN
{
    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Extension;

    using System.Collections.Generic;

    using UnityEngine;
    using XmobiTea.EUN.Constant;

    /// <summary>
    /// What is EUNView?
    /// This is agent handle for room game object
    /// Each EUNView represents a room game object
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class EUNView : Behaviour
    {
        [SerializeField]
        private RoomGameObject roomGameObject;
        /// <summary>
        /// The room game object for this EUNView
        /// </summary>
        public RoomGameObject RoomGameObject
        {
            get { return roomGameObject; }
            private set
            {
                roomGameObject = value;
            }
        }

        /// <summary>
        /// Every EUNView has multiple eun behaviour
        /// </summary>
        internal List<EUNBehaviour> eunBehaviourLst { get; private set; } = new List<EUNBehaviour>();

        /// <summary>
        /// Every EUNView has multiple eun manager behaviour
        /// </summary>
        internal List<EUNManagerBehaviour> eunManagerBehaviourLst { get; private set; } = new List<EUNManagerBehaviour>();

        /// <summary>
        /// Every EUNView has multiple eun voice chat behaviour
        /// </summary>
        internal List<EUNVoiceChatBehaviour> eunVoiceChatBehaviourLst { get; private set; } = new List<EUNVoiceChatBehaviour>();

        /// <summary>
        /// Get Owner room player for this EUNView
        /// It can be null
        /// </summary>
        public RoomPlayer Owner => EUNNetwork.GetRoomPlayer(RoomGameObject.OwnerId);

        /// <summary>
        /// Check if EUNView is mine
        /// </summary>
        public bool IsMine => RoomGameObject != null && RoomGameObject.OwnerId == EUNNetwork.PlayerId;

        protected override void OnCustomStart()
        {
            base.OnCustomStart();
            
            if (RoomGameObject.IsValid())
                EUNNetwork.SubscriberEUNView(this);
        }

        protected override void OnCustomDestroy()
        {
            base.OnCustomDestroy();

            EUNNetwork.UnSubscriberEUNView(this);
        }

        /// <summary>
        /// Init the room game object for this EUN View
        /// </summary>
        /// <param name="roomGameObject"></param>
        internal void Init(RoomGameObject roomGameObject)
        {
            this.RoomGameObject = roomGameObject;

            EUNNetwork.SubscriberEUNView(this);

            if (RoomGameObject.IsValid())
            {
                for (var i = 0; i < eunBehaviourLst.Count; i++)
                {
                    var behaviour = eunBehaviourLst[i];
                    if (behaviour != null)
                    {
                        behaviour.OnEUNInitialize(RoomGameObject.InitializeData);
                        behaviour.OnEUNSynchronization(RoomGameObject.SynchronizationData);
                    }
                }
            }
        }

        /// <summary>
        /// Subscriber a EUNBehaviour behaviour
        /// </summary>
        /// <param name="behaviour"></param>
        internal void SubscriberEUNBehaviour(EUNBehaviour behaviour)
        {
            if (!eunBehaviourLst.Contains(behaviour))
            {
                eunBehaviourLst.Add(behaviour);

                if (RoomGameObject.IsValid())
                {
                    behaviour.OnEUNInitialize(RoomGameObject.InitializeData);
                    behaviour.OnEUNSynchronization(RoomGameObject.SynchronizationData);
                }
            }
        }

        /// <summary>
        /// Subscriber a EUNManagerBehaviour behaviour
        /// </summary>
        /// <param name="behaviour"></param>
        internal void SubscriberEUNManagerBehaviour(EUNManagerBehaviour behaviour)
        {
            if (!eunManagerBehaviourLst.Contains(behaviour)) eunManagerBehaviourLst.Add(behaviour);
        }

        /// <summary>
        /// Subscriber a EUNVoiceChatBehaviour behaviour
        /// </summary>
        /// <param name="behaviour"></param>
        internal void SubscriberEUNBehaviour(EUNVoiceChatBehaviour behaviour)
        {
            if (!eunVoiceChatBehaviourLst.Contains(behaviour)) eunVoiceChatBehaviourLst.Add(behaviour);
        }

        /// <summary>
        /// Remove subscriber EUNBehaviour
        /// </summary>
        /// <param name="behaviour"></param>
        internal void UnSubscriberEUNBehaviour(EUNBehaviour behaviour)
        {
            if (eunBehaviourLst.Contains(behaviour)) eunBehaviourLst.Remove(behaviour);
        }

        /// <summary>
        /// Remove subscriber EUNManagerBehaviour
        /// </summary>
        /// <param name="behaviour"></param>
        internal void UnSubscriberEUNManagerBehaviour(EUNManagerBehaviour behaviour)
        {
            if (eunManagerBehaviourLst.Contains(behaviour)) eunManagerBehaviourLst.Remove(behaviour);
        }

        /// <summary>
        /// Remove subscriber EUNVoiceChatBehaviour
        /// </summary>
        /// <param name="behaviour"></param>
        internal void UnSubscriberEUNBehaviour(EUNVoiceChatBehaviour behaviour)
        {
            if (eunVoiceChatBehaviourLst.Contains(behaviour)) eunVoiceChatBehaviourLst.Remove(behaviour);
        }

        /// <summary>
        /// RPC call
        /// </summary>
        /// <param name="targets">The targets room player want to receive RPC</param>
        /// <param name="command">The command</param>
        /// <param name="rpcData">The data rpc</param>
        public void RPC(EUNTargets targets, EUNRPCCommand command, params object[] rpcData)
        {
            EUNNetworkExtensions.RPC(this, targets, command, rpcData);
        }
    }
}
