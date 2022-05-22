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

    [RequireComponent(typeof(EUNView))]
    public class EUNBehaviour : MonoBehaviour
    {
        private static Dictionary<Type, MethodInfo[]> methodInfoDic = new Dictionary<Type, MethodInfo[]>();

        public EUNView eunView { get; private set; }

        protected virtual void Awake()
        {
            if (eunView == null) eunView = GetComponent<EUNView>();
        }

        protected virtual void Start()
        {
            if (eunView != null) eunView.SubscriberEUNBehaviour(this);
        }

        protected virtual void OnDisable()
        {

        }

        protected virtual void OnDestroy()
        {
            if (eunView != null) eunView.UnSubscriberEUNBehaviour(this);
        }

        public void EUNRPC(int eunRPCCommand, EUNArray rpcDataArray)
        {
            var type = GetType();

            MethodInfo[] methodInfos;
            
            if (methodInfoDic.ContainsKey(type))
            {
                methodInfos = methodInfoDic[type];
            }
            else
            {
                methodInfos = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).Where(x => x.GetCustomAttributes(typeof(EUNRPCAttribute), true).Length > 0).ToArray();
                methodInfoDic[type] = methodInfos;
            }

            var eunRPCMethodName = ((EUNRPCCommand)eunRPCCommand).ToString();

            MethodInfo method = null;
            object[] parameters = null;// = rpcData != null ? rpcData.toList<object>().ToArray() : new object[0];

            foreach (var methodInfo in methodInfos)
            {
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

            if (method != null) method.Invoke(this, parameters);
            else
            {
                EUNDebug.LogError("Method " + eunRPCMethodName + " with parameters " + parameters + " not found");
            }
        }

        [EUNRPC]
        private void None() { }

        public virtual void OnEUNCustomPlayerPropertiesChange(RoomPlayer player, EUNHashtable customPropertiesChange) { }

        public virtual void OnEUNCustomGameObjectPropertiesChange(EUNHashtable customPropertiesChange) { }

        public virtual void OnEUNCustomRoomPropertiesChange(EUNHashtable customPropertiesChange) { }

        public virtual void OnEUNLeaderClientChange(RoomPlayer newLeaderClientPlayer) { }

        public virtual void OnEUNOtherPlayerJoinRoom(RoomPlayer player) { }

        public virtual void OnEUNOtherPlayerLeftRoom(RoomPlayer player) { }

        public virtual void OnEUNReceiveChatRoom(ChatMessage message, RoomPlayer sender) { }

        public virtual void OnEUNRoomInfoChange(EUNHashtable customPropertiesChange) { }

        public virtual void OnEUNInitialize(object initializeData) { }

        public virtual void OnEUNSynchronization(object synchronizationData) { }

        public virtual object GetSynchronizationData() { return null; }

        public virtual void OnEUNTransferOwnerGameObject(RoomPlayer newOwner) { }
    }
}
