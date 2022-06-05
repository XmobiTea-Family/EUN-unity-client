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
        private RoomGameObject _roomGameObject;
        /// <summary>
        /// The room game object for this EUNView
        /// </summary>
        public RoomGameObject roomGameObject
        {
            get { return _roomGameObject; }
            private set
            {
                _roomGameObject = value;
            }
        }

        /// <summary>
        /// Every EUNView has multiple eun behaviour
        /// </summary>
        internal List<EUNBehaviour> _eunBehaviourLst { get; private set; } = new List<EUNBehaviour>();

        [SerializeField]
        private ObservedComponent[] _observedComponentLst;
        internal ObservedComponent[] observedComponentLst => _observedComponentLst;

        /// <summary>
        /// Every EUNView has multiple eun manager behaviour
        /// </summary>
        internal List<EUNManagerBehaviour> _eunManagerBehaviourLst { get; private set; } = new List<EUNManagerBehaviour>();

        /// <summary>
        /// Every EUNView has multiple eun voice chat behaviour
        /// </summary>
        internal List<EUNVoiceChatBehaviour> _eunVoiceChatBehaviourLst { get; private set; } = new List<EUNVoiceChatBehaviour>();

        /// <summary>
        /// Get Owner room player for this EUNView
        /// It can be null
        /// </summary>
        public RoomPlayer owner => EUNNetwork.GetRoomPlayer(this.roomGameObject.ownerId);

        /// <summary>
        /// Check if EUNView is mine
        /// </summary>
        public bool isMine => this.roomGameObject != null && (this.roomGameObject.ownerId == EUNNetwork.playerId || this.roomGameObject.objectId < 0);

        protected override void OnCustomStart()
        {
            base.OnCustomStart();
            
            if (this.roomGameObject.IsValid())
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
            this.roomGameObject = roomGameObject;

            if (this.roomGameObject.IsValid())
            {
                EUNNetwork.SubscriberEUNView(this);

                for (var i = 0; i < _eunBehaviourLst.Count; i++)
                {
                    var behaviour = _eunBehaviourLst[i];
                    if (behaviour != null)
                    {
                        behaviour.OnEUNInitialize(this.roomGameObject.initializeData);
                        behaviour.OnEUNSynchronization(this.roomGameObject.synchronizationData);
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
            if (!_eunBehaviourLst.Contains(behaviour))
            {
                _eunBehaviourLst.Add(behaviour);

                if (roomGameObject.IsValid())
                {
                    behaviour.OnEUNInitialize(roomGameObject.initializeData);
                    behaviour.OnEUNSynchronization(roomGameObject.synchronizationData);
                }
            }
        }

        /// <summary>
        /// Subscriber a EUNManagerBehaviour behaviour
        /// </summary>
        /// <param name="behaviour"></param>
        internal void SubscriberEUNManagerBehaviour(EUNManagerBehaviour behaviour)
        {
            if (!_eunManagerBehaviourLst.Contains(behaviour)) _eunManagerBehaviourLst.Add(behaviour);
        }

        /// <summary>
        /// Subscriber a EUNVoiceChatBehaviour behaviour
        /// </summary>
        /// <param name="behaviour"></param>
        internal void SubscriberEUNBehaviour(EUNVoiceChatBehaviour behaviour)
        {
            if (!_eunVoiceChatBehaviourLst.Contains(behaviour)) _eunVoiceChatBehaviourLst.Add(behaviour);
        }

        /// <summary>
        /// Remove subscriber EUNBehaviour
        /// </summary>
        /// <param name="behaviour"></param>
        internal void UnSubscriberEUNBehaviour(EUNBehaviour behaviour)
        {
            if (_eunBehaviourLst.Contains(behaviour)) _eunBehaviourLst.Remove(behaviour);
        }

        /// <summary>
        /// Remove subscriber EUNManagerBehaviour
        /// </summary>
        /// <param name="behaviour"></param>
        internal void UnSubscriberEUNManagerBehaviour(EUNManagerBehaviour behaviour)
        {
            if (_eunManagerBehaviourLst.Contains(behaviour)) _eunManagerBehaviourLst.Remove(behaviour);
        }

        /// <summary>
        /// Remove subscriber EUNVoiceChatBehaviour
        /// </summary>
        /// <param name="behaviour"></param>
        internal void UnSubscriberEUNBehaviour(EUNVoiceChatBehaviour behaviour)
        {
            if (_eunVoiceChatBehaviourLst.Contains(behaviour)) _eunVoiceChatBehaviourLst.Remove(behaviour);
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
