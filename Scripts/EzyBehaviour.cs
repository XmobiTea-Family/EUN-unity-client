namespace EUN
{
    using EUN.Common;
    using EUN.Entity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEngine;

    [RequireComponent(typeof(EzyView))]
    public class EzyBehaviour : MonoBehaviour
    {
        public EzyView ezyView { get; private set; }

        protected virtual void Awake()
        {
            if (ezyView == null) ezyView = GetComponent<EzyView>();
        }

        private List<MethodInfo> methodInfoLst;

        protected virtual void Start()
        {
            if (ezyView != null) ezyView.SubscriberEzyBehaviour(this);
        }

        protected virtual void OnDisable()
        {

        }

        protected virtual void OnDestroy()
        {
            if (ezyView != null) ezyView.UnSubscriberEzyBehaviour(this);
        }

        public void EzyRPC(int eunRPCCommand, CustomArray rpcDataArray)
        {

            if (methodInfoLst == null) methodInfoLst = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).Where(x => x.GetCustomAttributes(typeof(EzyRPCAttribute), true).Length > 0).ToList();

            var ezyRPCMethodName = ((EzyRPCCommand)eunRPCCommand).ToString();

            MethodInfo method = null;
            object[] parameters = null;// = rpcData != null ? rpcData.toList<object>().ToArray() : new object[0];

            foreach (var methodInfo in methodInfoLst)
            {
                if (methodInfo.Name.Equals(ezyRPCMethodName))
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
                                    else if (parameterInfo.ParameterType == typeof(CustomArray))
                                    {
                                        tempParameters[i] = rpcDataArray.GetCustomArray(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(CustomHashtable))
                                    {
                                        tempParameters[i] = rpcDataArray.GetCustomHashtable(i);
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
                                Debug.LogException(ex);

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
                Debug.LogError("Method " + ezyRPCMethodName + " with parameters " + parameters + " not found");
            }
        }

        [EzyRPC]
        private void None() { }

        public virtual void OnEzyCustomPlayerPropertiesChange(RoomPlayer player, CustomHashtable customPropertiesChange) { }

        public virtual void OnEzyCustomGameObjectPropertiesChange(CustomHashtable customPropertiesChange) { }

        public virtual void OnEzyCustomRoomPropertiesChange(CustomHashtable customPropertiesChange) { }

        public virtual void OnEzyLeaderClientChange(RoomPlayer newLeaderClientPlayer) { }

        public virtual void OnEzyOtherPlayerJoinRoom(RoomPlayer player) { }

        public virtual void OnEzyOtherPlayerLeftRoom(RoomPlayer player) { }

        public virtual void OnEzyReceiveChatRoom(ChatMessage message, RoomPlayer sender) { }

        public virtual void OnEzyRoomInfoChange(CustomHashtable customPropertiesChange) { }

        public virtual void OnEzyInitialize(object initializeData) { }

        public virtual void OnEzySynchronization(object synchronizationData) { }

        public virtual object GetSynchronizationData() { return null; }

        public virtual void OnEzyTransferOwnerGameObject(RoomPlayer newOwner) { }
    }
}
