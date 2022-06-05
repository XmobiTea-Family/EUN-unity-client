namespace XmobiTea.EUN
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Entity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using XmobiTea.EUN.Logger;

    /// <summary>
    /// What is EUNBehaviour?
    /// This is controller for EUNView
    /// one EUNView may be have multiple EUNBehaviour
    /// </summary>
    public class EUNBehaviour : Behaviour
    {
        /// <summary>
        /// All method info dict
        /// </summary>
        private static Dictionary<Type, MethodInfo[]> methodInfoDic => new Dictionary<Type, MethodInfo[]>();
        
        protected override void OnCustomStart()
        {
            base.OnCustomStart();

            if (eunView) eunView.SubscriberEUNBehaviour(this);
        }

        protected override void OnCustomDestroy()
        {
            base.OnCustomDestroy();

            if (eunView) eunView.UnSubscriberEUNBehaviour(this);
        }

        /// <summary>
        /// This is RPC callback from EUNNetwork.RpcGameObjectRoom
        /// </summary>
        /// <param name="eunRPCCommand"></param>
        /// <param name="rpcDataArray"></param>
        internal void EUNRPC(int eunRPCCommand, EUNArray rpcDataArray)
        {
            var type = GetType();

            var methodInfos = EUNBehaviour.getMethodInfos(type);

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

        internal static bool tryGetMethod(MethodInfo[] methodInfos, string eunRPCMethodName, EUNArray rpcDataArray, out MethodInfo method, out object[] parameters)
        {
            method = null;
            parameters = null;

            for (var j = 0; j < methodInfos.Length; j++)
            {
                var methodInfo = methodInfos[j];

                if (methodInfo.Name.Equals(eunRPCMethodName))
                {
                    var parameterInfos = methodInfo.GetParameters();

                    if (parameterInfos.Length == rpcDataArray.Count())
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

                                    if (parameterInfo.ParameterType == typeof(bool))
                                    {
                                        tempParameters[i] = rpcDataArray.GetBool(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(sbyte))
                                    {
                                        tempParameters[i] = rpcDataArray.GetSByte(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(byte))
                                    {
                                        tempParameters[i] = rpcDataArray.GetByte(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(int))
                                    {
                                        tempParameters[i] = rpcDataArray.GetInt(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(short))
                                    {
                                        tempParameters[i] = rpcDataArray.GetShort(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(long))
                                    {
                                        tempParameters[i] = rpcDataArray.GetLong(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(float))
                                    {
                                        tempParameters[i] = rpcDataArray.GetFloat(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(double))
                                    {
                                        tempParameters[i] = rpcDataArray.GetDouble(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(string))
                                    {
                                        tempParameters[i] = rpcDataArray.GetString(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(EUNArray))
                                    {
                                        tempParameters[i] = rpcDataArray.GetEUNArray(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(EUNHashtable))
                                    {
                                        tempParameters[i] = rpcDataArray.GetEUNHashtable(i);
                                    }

                                    else if (parameterInfo.ParameterType == typeof(bool[]))
                                    {
                                        tempParameters[i] = rpcDataArray.GetArray<bool>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(sbyte[]))
                                    {
                                        tempParameters[i] = rpcDataArray.GetArray<sbyte>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(byte[]))
                                    {
                                        tempParameters[i] = rpcDataArray.GetArray<byte>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(int[]))
                                    {
                                        tempParameters[i] = rpcDataArray.GetArray<int>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(short[]))
                                    {
                                        tempParameters[i] = rpcDataArray.GetArray<short>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(long[]))
                                    {
                                        tempParameters[i] = rpcDataArray.GetArray<long>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(float[]))
                                    {
                                        tempParameters[i] = rpcDataArray.GetArray<float>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(double[]))
                                    {
                                        tempParameters[i] = rpcDataArray.GetArray<double>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(string[]))
                                    {
                                        tempParameters[i] = rpcDataArray.GetArray<string>(i);
                                    }

                                    else if (parameterInfo.ParameterType == typeof(IList<bool>))
                                    {
                                        tempParameters[i] = rpcDataArray.GetList<bool>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(IList<sbyte>))
                                    {
                                        tempParameters[i] = rpcDataArray.GetList<sbyte>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(IList<byte>))
                                    {
                                        tempParameters[i] = rpcDataArray.GetList<byte>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(IList<int>))
                                    {
                                        tempParameters[i] = rpcDataArray.GetList<int>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(IList<short>))
                                    {
                                        tempParameters[i] = rpcDataArray.GetList<short>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(IList<long>))
                                    {
                                        tempParameters[i] = rpcDataArray.GetList<long>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(IList<float>))
                                    {
                                        tempParameters[i] = rpcDataArray.GetList<float>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(IList<double>))
                                    {
                                        tempParameters[i] = rpcDataArray.GetList<double>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(IList<string>))
                                    {
                                        tempParameters[i] = rpcDataArray.GetList<string>(i);
                                    }
                                    else continue;
                                }

                                parameters = tempParameters;
                            }
                            catch (Exception ex)
                            {
                                EUNDebug.LogException(ex);

                                continue;
                            }
                        }

                        method = methodInfo;
                        break;
                    }
                }
            }

            return method != null;
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
        public virtual void OnEUNCustomPlayerPropertiesChange(RoomPlayer player, EUNHashtable customPropertiesChange) { }

        /// <summary>
        /// Callback if custom game object properties change
        /// </summary>
        /// <param name="customPropertiesChange">The room game object custom properties change</param>
        public virtual void OnEUNCustomGameObjectPropertiesChange(EUNHashtable customPropertiesChange) { }

        /// <summary>
        /// Callback if this room game object need destroy
        /// </summary>
        public virtual void OnEUNDestroyGameObjectRoom() { }

        /// <summary>
        /// Callback if custom room properties change
        /// </summary>
        /// <param name="customPropertiesChange">The custom room properties change</param>
        public virtual void OnEUNCustomRoomPropertiesChange(EUNHashtable customPropertiesChange) { }

        /// <summary>
        /// Callback if leader client change
        /// </summary>
        /// <param name="newLeaderClientPlayer">The new leader client</param>
        public virtual void OnEUNLeaderClientChange(RoomPlayer newLeaderClientPlayer) { }

        /// <summary>
        /// Callback if other player join room
        /// </summary>
        /// <param name="player">The player join this room</param>
        public virtual void OnEUNOtherPlayerJoinRoom(RoomPlayer player) { }

        /// <summary>
        /// Callback if other player left room
        /// </summary>
        /// <param name="player">The player left this room</param>
        public virtual void OnEUNOtherPlayerLeftRoom(RoomPlayer player) { }

        /// <summary>
        /// Callback if has chat room
        /// </summary>
        /// <param name="message">The message chat receive</param>
        /// <param name="sender">The sender of message</param>
        public virtual void OnEUNReceiveChatRoom(ChatMessage message, RoomPlayer sender) { }

        /// <summary>
        /// Callback if room info change
        /// </summary>
        /// <param name="customPropertiesChange">The custom properties change</param>
        public virtual void OnEUNRoomInfoChange(EUNHashtable customPropertiesChange) { }

        /// <summary>
        /// Callback if init room game object
        /// </summary>
        /// <param name="initializeData">The init data, it should as EUNArray</param>
        public virtual void OnEUNInitialize(object initializeData) { }

        /// <summary>
        /// Callback if sync request sent success from other client from room game object
        /// </summary>
        /// <param name="synchronizationData">The sync data, it should as EUNArray</param>
        public virtual void OnEUNSynchronization(object synchronizationData) { }

        /// <summary>
        /// Callback if EUN Client need get sync request for EUNBehaviour
        /// </summary>
        /// <returns>null to dont send this sync request</returns>
        public virtual object GetSynchronizationData() { return null; }

        /// <summary>
        /// Callback if the owner room game object EUNView was change
        /// </summary>
        /// <param name="newOwner">The new owner for this eunView</param>
        public virtual void OnEUNTransferOwnerGameObject(RoomPlayer newOwner) { }
    }
}
