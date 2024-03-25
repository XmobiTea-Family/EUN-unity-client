namespace XmobiTea.EUN
{
#if EUN_USING_ONLINE
    using com.tvd12.ezyfoxserver.client.constant;
#else
    using XmobiTea.EUN.Entity.Support;
#endif
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Entity;

    using UnityEngine;

    /// <summary>
    /// The EUNManagerBehaviour, manager all callback from server
    /// </summary>
    public class EUNManagerBehaviour : MonoBehaviour, IEUNManagerBehaviour
    {
        void Awake()
        {
            this.onCustomAwake();
        }

        void Start()
        {
            this.onCustomStart();
        }

        void OnEnable()
        {
            this.onCustomEnable();
        }

        void OnDisable()
        {
            this.onCustomDisable();
        }

        void OnDestroy()
        {
            this.onCustomDestroy();
        }

        /// <summary>
        /// This is a MonoBehaviour.Awake()
        /// </summary>
        protected virtual void onCustomAwake()
        {
            EUNNetwork.subscriberEUNManagerBehaviour(this);
        }

        /// <summary>
        /// This is a MonoBehaviour.OnEnable()
        /// </summary>
        protected virtual void onCustomEnable() { }

        /// <summary>
        /// This is a MonoBehaviour.Start()
        /// </summary>
        protected virtual void onCustomStart() { }

        /// <summary>
        /// This is a MonoBehaviour.OnDisable()
        /// </summary>
        protected virtual void onCustomDisable() { }

        /// <summary>
        /// This is a MonoBehaviour.OnDestroy()
        /// </summary>
        protected virtual void onCustomDestroy()
        {
            EUNNetwork.unSubscriberEUNManagerBehaviour(this);
        }

        /// <summary>
        /// Callback if custom player properties change
        /// </summary>
        /// <param name="player">The player has custom properties change</param>
        /// <param name="customPropertiesChange">The player custom properties change</param>
        public virtual void onEUNCustomPlayerPropertiesChange(RoomPlayer player, EUNHashtable customPropertiesChange) { }

        /// <summary>
        /// Callback if custom room properties change
        /// </summary>
        /// <param name="customPropertiesChange">The room custom properties change</param>
        public virtual void onEUNCustomRoomPropertiesChange(EUNHashtable customPropertiesChange) { }

        /// <summary>
        /// Callback if custom room game object properties change
        /// </summary>
        /// <param name="roomGameObject">The room game object has custom properties change</param>
        /// <param name="customPropertiesChange">The room game object custom properties change</param>
        public virtual void onEUNCustomGameObjectPropertiesChange(RoomGameObject roomGameObject, EUNHashtable customPropertiesChange) { }

        /// <summary>
        /// Callback if EUN Client connected to Zone
        /// </summary>
        public virtual void onEUNZoneConnected() { }

        /// <summary>
        /// Callback if EUN Client connected success to App and can send operation request
        /// </summary>
        public virtual void onEUNConnected() { }

        /// <summary>
        /// Callback if EUN Client login has error
        /// </summary>
        public virtual void onEUNLoginError() { }

        /// <summary>
        /// Callback if EUN Client not connect to EUN Server and connect failure
        /// </summary>
        /// <param name="reason">The reason connect failure</param>
        public virtual void onEUNConnectionFailure(EzyConnectionFailedReason reason) { }

        /// <summary>
        /// Callback if EUN Client connected to EUN Server but disconnect
        /// </summary>
        /// <param name="reason">The reason disconnect</param>
        public virtual void onEUNDisconnected(EzyDisconnectReason reason) { }

        /// <summary>
        /// Callback if client join lobby success
        /// </summary>
        public virtual void onEUNJoinLobby() { }

        /// <summary>
        /// Callback if client left lobby success
        /// </summary>
        public virtual void onEUNLeftLobby() { }

        /// <summary>
        /// Callback if client join room success
        /// </summary>
        public virtual void onEUNJoinRoom() { }

        /// <summary>
        /// Callback if new leader client for the room change
        /// </summary>
        /// <param name="newLeaderClientPlayer">The new leader client</param>
        public virtual void onEUNLeaderClientChange(RoomPlayer newLeaderClientPlayer) { }

        /// <summary>
        /// Callback if client left room success
        /// </summary>
        public virtual void onEUNLeftRoom() { }

        /// <summary>
        /// Callback if other player join room
        /// </summary>
        /// <param name="player">The player join room</param>
        public virtual void onEUNOtherPlayerJoinRoom(RoomPlayer player) { }

        /// <summary>
        /// Callback if other player left room
        /// </summary>
        /// <param name="player">The player left room</param>
        public virtual void onEUNOtherPlayerLeftRoom(RoomPlayer player) { }

        /// <summary>
        /// Callback if somebody send chat all and this client subscriber chat all
        /// </summary>
        /// <param name="message">The chat message receive</param>
        public virtual void onEUNReceiveChatAll(ChatMessage message) { }

        /// <summary>
        /// Callback if somebody in lobby send chat lobby and this client subscriber chat lobby
        /// </summary>
        /// <param name="message">The chat message receive</param>
        public virtual void onEUNReceiveChatLobby(ChatMessage message) { }

        /// <summary>
        /// Callback if somebody in room send chat room
        /// </summary>
        /// <param name="message">The chat message receive</param>
        /// <param name="sender">The sender send this chat</param>
        public virtual void onEUNReceiveChatRoom(ChatMessage message, RoomPlayer sender) { }

        /// <summary>
        /// Callback if room info change
        /// </summary>
        /// <param name="customPropertiesChange">The custom properties change</param>
        public virtual void onEUNRoomInfoChange(EUNHashtable customPropertiesChange) { }

        /// <summary>
        /// Callback to answer the client create a Unity Game Object for roomGameObject does not have EUNView
        /// </summary>
        /// <param name="roomGameObject">The room game object does not have EUNView</param>
        /// <returns>null if you dont want to create Unity Game Object, or non null is a Unity Game Object has EUNView</returns>
        public virtual EUNView onEUNViewNeedCreate(RoomGameObject roomGameObject) { return null; }

        /// <summary>
        /// Callback if room game object has new owner
        /// </summary>
        /// <param name="roomGameObject">The room game object has new owner</param>
        /// <param name="newOwner">The new owner for this room game object</param>
        public virtual void onEUNTransferOwnerGameObject(RoomGameObject roomGameObject, RoomPlayer newOwner) { }

        /// <summary>
        /// Callback if has room game object need destroy
        /// </summary>
        /// <param name="roomGameObject">The room game object need destroy</param>
        public virtual void onEUNDestroyGameObjectRoom(RoomGameObject roomGameObject) { }

    }

}
