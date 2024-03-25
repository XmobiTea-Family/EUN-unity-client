namespace XmobiTea.EUN
{
    using XmobiTea.EUN.Entity;

    using System.Collections.Generic;

    using UnityEngine;
    using XmobiTea.EUN.Constant;

    /// <summary>
    /// What is EUNView?
    /// This is agent handle for room game object
    /// Each EUNView represents a room game object
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class EUNView : MonoBehaviour
    {
        [SerializeField]
        private RoomGameObject _roomGameObject;
        /// <summary>
        /// The room game object for this EUNView
        /// </summary>
        public RoomGameObject roomGameObject
        {
            get { return this._roomGameObject; }
            private set
            {
                this._roomGameObject = value;
            }
        }

        /// <summary>
        /// Every EUNView has multiple eun behaviour
        /// </summary>
        internal List<EUNBehaviour> eunBehaviourLst { get; private set; } = new List<EUNBehaviour>();

        /// <summary>
        /// Every EUNView has multiple eun voice chat behaviour
        /// </summary>
        internal List<EUNVoiceChatBehaviour> eunVoiceChatBehaviourLst { get; private set; } = new List<EUNVoiceChatBehaviour>();

        /// <summary>
        /// Get Owner room player for this EUNView
        /// It can be null
        /// </summary>
        public RoomPlayer owner => EUNNetwork.getRoomPlayer(this.roomGameObject.ownerId);

        /// <summary>
        /// Check if EUNView is mine
        /// </summary>
        public bool isMine => this.roomGameObject != null && this.roomGameObject.ownerId == EUNNetwork.playerId;

        private void Awake()
        {
            EUNNetwork.subscriberEUNView(this);
        }

        private void OnDestroy()
        {
            EUNNetwork.unSubscriberEUNView(this);
        }

        /// <summary>
        /// Init the room game object for this EUN View
        /// </summary>
        /// <param name="roomGameObject"></param>
        internal void init(RoomGameObject roomGameObject)
        {
            this.roomGameObject = roomGameObject;

            if (this.roomGameObject.isValid())
            {
                for (var i = 0; i < this.eunBehaviourLst.Count; i++)
                {
                    var behaviour = this.eunBehaviourLst[i];
                    if (behaviour != null)
                    {
                        behaviour.onEUNInitialize(this.roomGameObject.initializeData);
                        behaviour.onEUNSynchronization(this.roomGameObject.synchronizationData);
                    }
                }
            }
        }

        /// <summary>
        /// Subscriber a EUNBehaviour behaviour
        /// </summary>
        /// <param name="behaviour"></param>
        internal void subscriberEUNBehaviour(EUNBehaviour behaviour)
        {
            if (!this.eunBehaviourLst.Contains(behaviour))
            {
                this.eunBehaviourLst.Add(behaviour);

                if (this.roomGameObject.isValid())
                {
                    behaviour.onEUNInitialize(roomGameObject.initializeData);
                    behaviour.onEUNSynchronization(roomGameObject.synchronizationData);
                }
            }
        }

        /// <summary>
        /// Subscriber a EUNVoiceChatBehaviour behaviour
        /// </summary>
        /// <param name="behaviour"></param>
        internal void subscriberEUNBehaviour(EUNVoiceChatBehaviour behaviour)
        {
            if (!this.eunVoiceChatBehaviourLst.Contains(behaviour))
            {
                this.eunVoiceChatBehaviourLst.Add(behaviour);
            }
        }

        /// <summary>
        /// Remove subscriber EUNBehaviour
        /// </summary>
        /// <param name="behaviour"></param>
        internal void unSubscriberEUNBehaviour(EUNBehaviour behaviour)
        {
            if (this.eunBehaviourLst.Contains(behaviour)) this.eunBehaviourLst.Remove(behaviour);
        }

        /// <summary>
        /// Remove subscriber EUNVoiceChatBehaviour
        /// </summary>
        /// <param name="behaviour"></param>
        internal void unSubscriberEUNBehaviour(EUNVoiceChatBehaviour behaviour)
        {
            if (this.eunVoiceChatBehaviourLst.Contains(behaviour)) this.eunVoiceChatBehaviourLst.Remove(behaviour);
        }

        /// <summary>
        /// RPC call
        /// </summary>
        /// <param name="targets">The targets room player want to receive RPC</param>
        /// <param name="command">The command</param>
        /// <param name="rpcData">The data rpc</param>
        public void rpc(EUNTargets targets, EUNRPCCommand command, params object[] rpcData)
        {
            EUNNetwork.rpcGameObjectRoom(targets, this.roomGameObject.objectId, (int)command, rpcData);
        }

    }

}
