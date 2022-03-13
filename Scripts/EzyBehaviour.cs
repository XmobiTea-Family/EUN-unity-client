namespace EUN
{
#if EUN
    using com.tvd12.ezyfoxserver.client.entity;
#endif

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
#if EUN
            if (ezyView == null) ezyView = GetComponent<EzyView>();
#endif
        }

        private List<MethodInfo> methodInfoLst;

        protected virtual void Start()
        {
#if EUN
            if (ezyView != null) ezyView.SubscriberEzyBehaviour(this);
#endif
        }

        protected virtual void OnDisable()
        {

        }

        protected virtual void OnDestroy()
        {
            if (ezyView != null) ezyView.UnSubscriberEzyBehaviour(this);
        }

#if EUN
        public void EzyRPC(int eunRPCCommand, EzyArray rpcData)
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

                    if (parameterInfos.Length == rpcData.size())
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

                                    if (parameterInfo.ParameterType == typeof(sbyte))
                                    {
                                        tempParameters[i] = rpcData.get<sbyte>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(bool))
                                    {
                                        tempParameters[i] = rpcData.get<bool>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(byte))
                                    {
                                        tempParameters[i] = rpcData.get<byte>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(int))
                                    {
                                        tempParameters[i] = rpcData.get<int>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(short))
                                    {
                                        tempParameters[i] = rpcData.get<short>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(long))
                                    {
                                        tempParameters[i] = rpcData.get<long>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(float))
                                    {
                                        tempParameters[i] = rpcData.get<float>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(double))
                                    {
                                        tempParameters[i] = rpcData.get<double>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(string))
                                    {
                                        tempParameters[i] = rpcData.get<string>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(CustomHashtable))
                                    {
                                        var obj = rpcData.get<object>(i);
                                        if (obj is EzyObject objEzyObject)
                                        {
                                            tempParameters[i] = new CustomHashtable(objEzyObject);
                                        }
                                        else if (obj is CustomHashtable customHashtable)
                                        {
                                            tempParameters[i] = customHashtable;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                    else if (parameterInfo.ParameterType == typeof(EzyArray))
                                    {
                                        tempParameters[i] = rpcData.get<EzyArray>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(EzyObject))
                                    {
                                        tempParameters[i] = rpcData.get<EzyObject>(i);
                                    }
                                    else if (parameterInfo.ParameterType == typeof(bool[]))
                                    {
                                        tempParameters[i] = rpcData.get<EzyArray>(i).toList<bool>().ToArray();
                                    }
                                    else if (parameterInfo.ParameterType == typeof(byte[]))
                                    {
                                        tempParameters[i] = rpcData.get<EzyArray>(i).toList<byte>().ToArray();
                                    }
                                    else if (parameterInfo.ParameterType == typeof(int[]))
                                    {
                                        tempParameters[i] = rpcData.get<EzyArray>(i).toList<int>().ToArray();
                                    }
                                    else if (parameterInfo.ParameterType == typeof(short[]))
                                    {
                                        tempParameters[i] = rpcData.get<EzyArray>(i).toList<short>().ToArray();
                                    }
                                    else if (parameterInfo.ParameterType == typeof(long[]))
                                    {
                                        tempParameters[i] = rpcData.get<EzyArray>(i).toList<long>().ToArray();
                                    }
                                    else if (parameterInfo.ParameterType == typeof(float[]))
                                    {
                                        tempParameters[i] = rpcData.get<EzyArray>(i).toList<float>().ToArray();
                                    }
                                    else if (parameterInfo.ParameterType == typeof(double[]))
                                    {
                                        tempParameters[i] = rpcData.get<EzyArray>(i).toList<double>().ToArray();
                                    }
                                    else if (parameterInfo.ParameterType == typeof(string[]))
                                    {
                                        tempParameters[i] = rpcData.get<EzyArray>(i).toList<string>().ToArray();
                                    }

                                    else if (parameterInfo.ParameterType == typeof(List<bool>))
                                    {
                                        tempParameters[i] = rpcData.get<EzyArray>(i).toList<bool>();
                                    }
                                    else if (parameterInfo.ParameterType == typeof(List<byte>))
                                    {
                                        tempParameters[i] = rpcData.get<EzyArray>(i).toList<byte>();
                                    }
                                    else if (parameterInfo.ParameterType == typeof(List<int>))
                                    {
                                        tempParameters[i] = rpcData.get<EzyArray>(i).toList<int>();
                                    }
                                    else if (parameterInfo.ParameterType == typeof(List<short>))
                                    {
                                        tempParameters[i] = rpcData.get<EzyArray>(i).toList<short>();
                                    }
                                    else if (parameterInfo.ParameterType == typeof(List<long>))
                                    {
                                        tempParameters[i] = rpcData.get<EzyArray>(i).toList<long>();
                                    }
                                    else if (parameterInfo.ParameterType == typeof(List<float>))
                                    {
                                        tempParameters[i] = rpcData.get<EzyArray>(i).toList<float>();
                                    }
                                    else if (parameterInfo.ParameterType == typeof(List<double>))
                                    {
                                        tempParameters[i] = rpcData.get<EzyArray>(i).toList<double>();
                                    }
                                    else if (parameterInfo.ParameterType == typeof(List<string>))
                                    {
                                        tempParameters[i] = rpcData.get<EzyArray>(i).toList<string>();
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
#endif

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
