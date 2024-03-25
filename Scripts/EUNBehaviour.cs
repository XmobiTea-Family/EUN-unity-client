namespace XmobiTea.EUN
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Entity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEngine;
    using XmobiTea.EUN.Logger;

    /// <summary>
    /// What is EUNBehaviour?
    /// This is controller for EUNView
    /// one EUNView may be have multiple EUNBehaviour
    /// </summary>
    [RequireComponent(typeof(EUNView))]
    public class EUNBehaviour : MonoBehaviour
    {
        /// <summary>
        /// All method info dict
        /// </summary>
        private static Dictionary<Type, MethodInfo[]> methodInfoDict = new Dictionary<Type, MethodInfo[]>();

        /// <summary>
        /// The eunView for this EUN Behaviour
        /// </summary>
        public EUNView eunView { get; private set; }

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
            if (this.eunView == null) this.eunView = GetComponent<EUNView>();
        }

        /// <summary>
        /// This is a MonoBehaviour.Start()
        /// </summary>
        protected virtual void onCustomStart()
        {
            if (this.eunView != null) this.eunView.subscriberEUNBehaviour(this);
        }

        /// <summary>
        /// This is a MonoBehaviour.OnEnable()
        /// </summary>
        protected virtual void onCustomEnable()
        {

        }

        /// <summary>
        /// This is a MonoBehaviour.OnDisable()
        /// </summary>
        protected virtual void onCustomDisable()
        {

        }

        /// <summary>
        /// This is a MonoBehaviour.OnDestroy()
        /// </summary>
        protected virtual void onCustomDestroy()
        {
            if (this.eunView != null) this.eunView.unSubscriberEUNBehaviour(this);
        }

        /// <summary>
        /// This is RPC callback from EUNNetwork.RpcGameObjectRoom
        /// </summary>
        /// <param name="eunRPCCommand"></param>
        /// <param name="rpcDataArray"></param>
        internal void eunRpc(int eunRPCCommand, EUNArray rpcDataArray)
        {
            var type = GetType();

            MethodInfo[] methodInfos;
            
            if (methodInfoDict.ContainsKey(type))
            {
                methodInfos = methodInfoDict[type];
            }
            else
            {
                methodInfos = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).Where(x => x.GetCustomAttributes(typeof(EUNRPCAttribute), true).Length > 0).ToArray();
                methodInfoDict[type] = methodInfos;
            }

            var eunRPCMethodName = ((EUNRPCCommand)eunRPCCommand).ToString();

            MethodInfo method = null;
            object[] parameters = null;// = rpcData != null ? rpcData.toList<object>().ToArray() : new object[0];

            var methodCorrectInfos = methodInfos.Where(x => x.Name.Equals(eunRPCMethodName));

            foreach (var methodInfo in methodCorrectInfos)
            {
                var parameterInfos = methodInfo.GetParameters();

                if (parameterInfos.Length == rpcDataArray.count())
                {
                    if (parameterInfos.Length == 0)
                    {
                        parameters = new object[] { };
                    }
                    else
                    {
                        var tempParameters = new object[parameterInfos.Length];

                        try
                        {
                            for (var i = 0; i < parameterInfos.Length; i++)
                            {
                                var parameterInfo = parameterInfos[i];

                                if (parameterInfo.ParameterType == typeof(EUNView) || parameterInfo.ParameterType == typeof(RoomGameObject))
                                {
                                    var viewId = rpcDataArray.getInt(i);

                                    EUNNetwork.peer.eunViewDict.TryGetValue(viewId, out var view);

                                    if (view != null)
                                    {
                                        if (parameterInfo.ParameterType == typeof(EUNView)) tempParameters[i] = view;
                                        else tempParameters[i] = view.roomGameObject;
                                    }
                                    else continue;
                                }
                                else if (parameterInfo.ParameterType == typeof(RoomPlayer))
                                {
                                    var playerId = rpcDataArray.getInt(i);

                                    var roomPlayer = EUNNetwork.roomPlayerLst.Find(x => x.playerId == playerId);
                                    if (roomPlayer != null)
                                    {
                                        tempParameters[i] = roomPlayer;
                                    }
                                    else continue;
                                }
                                else if (parameterInfo.ParameterType == typeof(bool))
                                {
                                    tempParameters[i] = rpcDataArray.getBool(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(sbyte))
                                {
                                    tempParameters[i] = rpcDataArray.getSByte(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(byte))
                                {
                                    tempParameters[i] = rpcDataArray.getByte(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(int))
                                {
                                    tempParameters[i] = rpcDataArray.getInt(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(short))
                                {
                                    tempParameters[i] = rpcDataArray.getShort(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(long))
                                {
                                    tempParameters[i] = rpcDataArray.getLong(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(float))
                                {
                                    tempParameters[i] = rpcDataArray.getFloat(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(double))
                                {
                                    tempParameters[i] = rpcDataArray.getDouble(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(string))
                                {
                                    tempParameters[i] = rpcDataArray.getString(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(EUNArray))
                                {
                                    tempParameters[i] = rpcDataArray.getEUNArray(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(EUNHashtable))
                                {
                                    tempParameters[i] = rpcDataArray.getEUNHashtable(i);
                                }

                                else if (parameterInfo.ParameterType == typeof(bool[]))
                                {
                                    tempParameters[i] = rpcDataArray.getArray<bool>(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(sbyte[]))
                                {
                                    tempParameters[i] = rpcDataArray.getArray<sbyte>(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(byte[]))
                                {
                                    tempParameters[i] = rpcDataArray.getArray<byte>(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(int[]))
                                {
                                    tempParameters[i] = rpcDataArray.getArray<int>(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(short[]))
                                {
                                    tempParameters[i] = rpcDataArray.getArray<short>(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(long[]))
                                {
                                    tempParameters[i] = rpcDataArray.getArray<long>(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(float[]))
                                {
                                    tempParameters[i] = rpcDataArray.getArray<float>(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(double[]))
                                {
                                    tempParameters[i] = rpcDataArray.getArray<double>(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(string[]))
                                {
                                    tempParameters[i] = rpcDataArray.getArray<string>(i);
                                }

                                else if (parameterInfo.ParameterType == typeof(IList<bool>))
                                {
                                    tempParameters[i] = rpcDataArray.getList<bool>(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(IList<sbyte>))
                                {
                                    tempParameters[i] = rpcDataArray.getList<sbyte>(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(IList<byte>))
                                {
                                    tempParameters[i] = rpcDataArray.getList<byte>(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(IList<int>))
                                {
                                    tempParameters[i] = rpcDataArray.getList<int>(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(IList<short>))
                                {
                                    tempParameters[i] = rpcDataArray.getList<short>(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(IList<long>))
                                {
                                    tempParameters[i] = rpcDataArray.getList<long>(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(IList<float>))
                                {
                                    tempParameters[i] = rpcDataArray.getList<float>(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(IList<double>))
                                {
                                    tempParameters[i] = rpcDataArray.getList<double>(i);
                                }
                                else if (parameterInfo.ParameterType == typeof(IList<string>))
                                {
                                    tempParameters[i] = rpcDataArray.getList<string>(i);
                                }
                                else continue;
                            }

                            parameters = tempParameters;
                        }
                        catch (Exception ex)
                        {
                            EUNDebug.logException(ex);

                            continue;
                        }
                    }

                    method = methodInfo;
                    break;
                }
            }

            if (method != null) method.Invoke(this, parameters);
            else
            {
                EUNDebug.logError("Method " + eunRPCMethodName + " with parameters " + parameters + " not found");
            }
        }

        /// <summary>
        /// The default RPC
        /// </summary>
        [EUNRPC]
        private void None() { }

        /// <summary>
        /// Callback if custom player properties change
        /// </summary>
        /// <param name="player"></param>
        /// <param name="customPropertiesChange">The player custom properties change</param>
        public virtual void onEUNCustomPlayerPropertiesChange(RoomPlayer player, EUNHashtable customPropertiesChange) { }

        /// <summary>
        /// Callback if custom game object properties change
        /// </summary>
        /// <param name="customPropertiesChange">The room game object custom properties change</param>
        public virtual void onEUNCustomGameObjectPropertiesChange(EUNHashtable customPropertiesChange) { }

        /// <summary>
        /// Callback if this room game object need destroy
        /// </summary>
        public virtual void onEUNDestroyGameObjectRoom() { }

        /// <summary>
        /// Callback if custom room properties change
        /// </summary>
        /// <param name="customPropertiesChange">The custom room properties change</param>
        public virtual void onEUNCustomRoomPropertiesChange(EUNHashtable customPropertiesChange) { }

        /// <summary>
        /// Callback if leader client change
        /// </summary>
        /// <param name="newLeaderClientPlayer">The new leader client</param>
        public virtual void onEUNLeaderClientChange(RoomPlayer newLeaderClientPlayer) { }

        /// <summary>
        /// Callback if other player join room
        /// </summary>
        /// <param name="player">The player join this room</param>
        public virtual void onEUNOtherPlayerJoinRoom(RoomPlayer player) { }

        /// <summary>
        /// Callback if other player left room
        /// </summary>
        /// <param name="player">The player left this room</param>
        public virtual void onEUNOtherPlayerLeftRoom(RoomPlayer player) { }

        /// <summary>
        /// Callback if has chat room
        /// </summary>
        /// <param name="message">The message chat receive</param>
        /// <param name="sender">The sender of message</param>
        public virtual void onEUNReceiveChatRoom(ChatMessage message, RoomPlayer sender) { }

        /// <summary>
        /// Callback if room info change
        /// </summary>
        /// <param name="customPropertiesChange">The custom properties change</param>
        public virtual void onEUNRoomInfoChange(EUNHashtable customPropertiesChange) { }

        /// <summary>
        /// Callback if init room game object
        /// </summary>
        /// <param name="initializeData">The init data, it should as EUNArray</param>
        public virtual void onEUNInitialize(object initializeData) { }

        /// <summary>
        /// Callback if sync request sent success from other client from room game object
        /// </summary>
        /// <param name="synchronizationData">The sync data, it should as EUNArray</param>
        public virtual void onEUNSynchronization(object synchronizationData) { }

        /// <summary>
        /// Callback if EUN Client need get sync request for EUNBehaviour
        /// </summary>
        /// <returns>null to dont send this sync request</returns>
        public virtual object getSynchronizationData() { return null; }

        /// <summary>
        /// Callback if the owner room game object EUNView was change
        /// </summary>
        /// <param name="newOwner">The new owner for this eunView</param>
        public virtual void onEUNTransferOwnerGameObject(RoomPlayer newOwner) { }

    }

}
