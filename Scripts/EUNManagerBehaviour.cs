namespace XmobiTea.EUN
{
#if EUN
    using com.tvd12.ezyfoxserver.client.constant;
#else
    using XmobiTea.EUN.Entity.Support;
#endif
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Entity;

    using System.Collections.Generic;
    using System.Reflection;
    using System;
    using XmobiTea.EUN.Logger;
    using System.Linq;

    /// <summary>
    /// The EUNManagerBehaviour, manager all callback from server
    /// </summary>
    public class EUNManagerBehaviour : Behaviour, IEUNManagerBehaviour
    {
        private static Dictionary<Type, MethodInfo[]> methodInfoDic => new Dictionary<Type, MethodInfo[]>();

        /// <summary>
        /// This is RPC callback from EUNNetwork.RpcGameObjectRoom
        /// </summary>
        /// <param name="eunRPCCommand"></param>
        /// <param name="rpcDataArray"></param>
        internal void EUNRPC(int eunRPCCommand, EUNArray rpcDataArray)
        {
            var type = GetType();

            var methodInfos = EUNManagerBehaviour.getMethodInfos(type);

            var eunRPCMethodName = ((EUNRPCCommand)eunRPCCommand).ToString();

            if (EUNBehaviour.tryGetMethod(methodInfos, eunRPCMethodName, rpcDataArray, out MethodInfo method, out object[] parameters))
            {
                method.Invoke(this, parameters);
            }
            else
            {
                EUNDebug.LogError("Method " + eunRPCMethodName + " with parameters " + parameters + " not found");
            }
        }

        private static MethodInfo[] getMethodInfos(Type type)
        {
            if (methodInfoDic.ContainsKey(type))
            {
                return methodInfoDic[type];
            }
            else
            {
                var methodInfos = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).Where(x => x.GetCustomAttributes(typeof(EUNRPCAttribute), true).Length > 0).ToArray();
                methodInfoDic[type] = methodInfos;

                return methodInfos;
            }
        }

        protected override void OnCustomStart()
        {
            base.OnCustomStart();
            
            EUNNetwork.SubscriberEUNManagerBehaviour(this);

            if (eunView) eunView.SubscriberEUNManagerBehaviour(this);
        }

        protected override void OnCustomDestroy()
        {
            base.OnCustomDestroy();

            EUNNetwork.UnSubscriberEUNManagerBehaviour(this);

            if (eunView) eunView.UnSubscriberEUNManagerBehaviour(this);
        }

        /// <summary>
        /// Callback if custom player properties change
        /// </summary>
        /// <param name="player">The player has custom properties change</param>
        /// <param name="customPropertiesChange">The player custom properties change</param>
        public virtual void OnEUNCustomPlayerPropertiesChange(RoomPlayer player, EUNHashtable customPropertiesChange) { }

        /// <summary>
        /// Callback if custom room properties change
        /// </summary>
        /// <param name="customPropertiesChange">The room custom properties change</param>
        public virtual void OnEUNCustomRoomPropertiesChange(EUNHashtable customPropertiesChange) { }

        /// <summary>
        /// Callback if custom room game object properties change
        /// </summary>
        /// <param name="roomGameObject">The room game object has custom properties change</param>
        /// <param name="customPropertiesChange">The room game object custom properties change</param>
        public virtual void OnEUNCustomGameObjectPropertiesChange(RoomGameObject roomGameObject, EUNHashtable customPropertiesChange) { }

        /// <summary>
        /// Callback if EUN Client connected to Zone
        /// </summary>
        public virtual void OnEUNZoneConnected() { }

        /// <summary>
        /// Callback if EUN Client connected success to App and can send operation request
        /// </summary>
        public virtual void OnEUNConnected() { }

        /// <summary>
        /// Callback if EUN Client login has error
        /// </summary>
        public virtual void OnEUNLoginError() { }

        /// <summary>
        /// Callback if EUN Client not connect to EUN Server and connect failure
        /// </summary>
        /// <param name="reason">The reason connect failure</param>
        public virtual void OnEUNConnectionFailure(EzyConnectionFailedReason reason) { }

        /// <summary>
        /// Callback if EUN Client connected to EUN Server but disconnect
        /// </summary>
        /// <param name="reason">The reason disconnect</param>
        public virtual void OnEUNDisconnected(EzyDisconnectReason reason) { }

        /// <summary>
        /// Callback if client join lobby success
        /// </summary>
        public virtual void OnEUNJoinLobby() { }

        /// <summary>
        /// Callback if client left lobby success
        /// </summary>
        public virtual void OnEUNLeftLobby() { }

        /// <summary>
        /// Callback if client join room success
        /// </summary>
        public virtual void OnEUNJoinRoom() { }

        /// <summary>
        /// Callback if new leader client for the room change
        /// </summary>
        /// <param name="newLeaderClientPlayer">The new leader client</param>
        public virtual void OnEUNLeaderClientChange(RoomPlayer newLeaderClientPlayer) { }

        /// <summary>
        /// Callback if client left room success
        /// </summary>
        public virtual void OnEUNLeftRoom() { }

        /// <summary>
        /// Callback if other player join room
        /// </summary>
        /// <param name="player">The player join room</param>
        public virtual void OnEUNOtherPlayerJoinRoom(RoomPlayer player) { }

        /// <summary>
        /// Callback if other player left room
        /// </summary>
        /// <param name="player">The player left room</param>
        public virtual void OnEUNOtherPlayerLeftRoom(RoomPlayer player) { }

        /// <summary>
        /// Callback if somebody send chat all and this client subscriber chat all
        /// </summary>
        /// <param name="message">The chat message receive</param>
        public virtual void OnEUNReceiveChatAll(ChatMessage message) { }

        /// <summary>
        /// Callback if somebody in lobby send chat lobby and this client subscriber chat lobby
        /// </summary>
        /// <param name="message">The chat message receive</param>
        public virtual void OnEUNReceiveChatLobby(ChatMessage message) { }

        /// <summary>
        /// Callback if somebody in room send chat room
        /// </summary>
        /// <param name="message">The chat message receive</param>
        /// <param name="sender">The sender send this chat</param>
        public virtual void OnEUNReceiveChatRoom(ChatMessage message, RoomPlayer sender) { }

        /// <summary>
        /// Callback if room info change
        /// </summary>
        /// <param name="customPropertiesChange">The custom properties change</param>
        public virtual void OnEUNRoomInfoChange(EUNHashtable customPropertiesChange) { }

        /// <summary>
        /// Callback to answer the client create a Unity Game Object for roomGameObject does not have EUNView
        /// </summary>
        /// <param name="roomGameObject">The room game object does not have EUNView</param>
        /// <returns>null if you dont want to create Unity Game Object, or non null is a Unity Game Object has EUNView</returns>
        public virtual EUNView OnEUNViewNeedCreate(RoomGameObject roomGameObject) { return null; }

        /// <summary>
        /// Callback if room game object has new owner
        /// </summary>
        /// <param name="roomGameObject">The room game object has new owner</param>
        /// <param name="newOwner">The new owner for this room game object</param>
        public virtual void OnEUNTransferOwnerGameObject(RoomGameObject roomGameObject, RoomPlayer newOwner) { }

        /// <summary>
        /// Callback if has room game object need destroy
        /// </summary>
        /// <param name="roomGameObject">The room game object need destroy</param>
        public virtual void OnEUNDestroyGameObjectRoom(RoomGameObject roomGameObject) { }
    }
}
