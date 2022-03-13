namespace EUN.Networking
{
#if EUN
    using com.tvd12.ezyfoxserver.client.entity;
    using com.tvd12.ezyfoxserver.client.factory;

    using EUN.Bride;
    using EUN.Bride.Socket;
    using EUN.Bride.WebSocket;
#endif
    using EUN.Common;
    using EUN.Constant;

    using System;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.SceneManagement;

    public partial class NetworkingPeer : MonoBehaviour
    {
        public struct OperationPending
        {
            private OperationRequest operationRequest;
            private Action<OperationResponse> onOperationResponse;

            public OperationPending(OperationRequest operationRequest, Action<OperationResponse> onOperationResponse)
            {
                this.operationRequest = operationRequest;
                this.onOperationResponse = onOperationResponse;
            }

            public OperationRequest GetOperationRequest()
            {
                return operationRequest;
            }

            public Action<OperationResponse> GetCallback()
            {
                return onOperationResponse;
            }
        }
#if EUN
        private IEzySocketObject ezySocketObject;
#endif

        private int requestId;

        private Queue<OperationPending> operationPendingQueue;
        private Queue<OperationPending> syncOperationPendingQueue;
        private Queue<OperationPending> voiceChatOperationPendingQueue;

        private Dictionary<int, OperationPending> operationWaitingResponseDic;

        private double serverTimeStamp;

        private float perMsgTimer;
        private float perSyncMsgTimer;
        private float perVoiceChatMsgTimer;

        private float nextSendMsgTimer;
        private float nextSendSyncMsgTimer;
        private float nextSendVoiceChatMsgTimer;

        internal void InitPeer()
        {
#if EUN
            serverTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if (operationWaitingResponseDic == null) operationWaitingResponseDic = new Dictionary<int, OperationPending>();
            if (operationPendingQueue == null) operationPendingQueue = new Queue<OperationPending>();
            if (syncOperationPendingQueue == null) syncOperationPendingQueue = new Queue<OperationPending>();
            if (voiceChatOperationPendingQueue == null) voiceChatOperationPendingQueue = new Queue<OperationPending>();

            if (serverEventHandlerDic == null) serverEventHandlerDic = new Dictionary<int, IServerEventHandler>();
            if (ezyViewLst == null) ezyViewLst = new List<EzyView>();
            if (ezyManagerBehaviourLst == null) ezyManagerBehaviourLst = new List<EzyManagerBehaviour>();
            if (ezyViewDic == null) ezyViewDic = new Dictionary<int, EzyView>();

            InitSendRate();

            InitEzySocketObject();

            SubscriberHandler();

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;

            SubscriberServerEventHandler();
#endif
        }

        private void InitSendRate()
        {
#if EUN
            var ezyServerSettings = EzyNetwork.ezyServerSettings;
            if (ezyServerSettings == null) throw new NullReferenceException("Null Ezy Server Settings, please find it now");

            var sendRate = ezyServerSettings.sendRate;
            var sendRateSynchronizationData = ezyServerSettings.sendRateSynchronizationData;
            var sendRateVoiceChat = ezyServerSettings.sendRateVoiceChat;

            SetSendRate(sendRate, sendRateSynchronizationData, sendRateVoiceChat);
#endif
        }

        private void InitEzySocketObject()
        {
#if EUN
            var ezyServerSettings = EzyNetwork.ezyServerSettings;

            if (ezyServerSettings == null) throw new NullReferenceException("Where is Ezy Server Settings");

#if !UNITY_EDITOR && UNITY_WEBGL
            ezySocketObject = gameObject.AddComponent<WebSocketEzySocketObject>();
#else
            ezySocketObject = gameObject.AddComponent<SocketEzySocketObject>();
#endif

            var zoneName = ezyServerSettings.zoneName;
            var appName = ezyServerSettings.appName;

            ezySocketObject.Init(zoneName, appName);
#endif
        }

        private void SubscriberHandler()
        {
#if EUN
            ezySocketObject.SubscriberConnectionSuccessHandler(OnConnectionSuccessHandler);
            ezySocketObject.SubscriberConnectionFailureHandler(OnConnectionFailureHandler);
            ezySocketObject.SubscriberDisconnectionHandler(OnDisconnectionHandler);
            ezySocketObject.SubscriberLoginErrorHandler(OnLoginErrorHandler);
            ezySocketObject.SubscriberAppAccessHandler(OnAppAccessHandler);
            ezySocketObject.SubscriberResponseHandler(OnResponseHandler);
            ezySocketObject.SubscriberEventHandler(OnEventHandler);
#endif
        }
#if EUN
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            StartCoroutine(IEOnSceneLoaded(scene, loadSceneMode));
        }

        System.Collections.IEnumerator IEOnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            yield return new WaitForSeconds(0.1f);

            var roomGameObjectNeedCreateLst = getListGameObjectNeedCreate();

            if (roomGameObjectNeedCreateLst != null && roomGameObjectNeedCreateLst.Count != 0)
            {
                foreach (var behaviour in ezyManagerBehaviourLst)
                {
                    if (behaviour)
                    {
                        foreach (var roomGameObject in roomGameObjectNeedCreateLst)
                        {
                            var view = behaviour.OnEzyViewNeedCreate(roomGameObject);
                            if (view != null)
                            {
                                view.Init(roomGameObject);
                                ezyViewDic[view.RoomGameObject.ObjectId] = view;
                            }
                        }
                    }
                }
            }
        }
#endif
#if EUN
        float checkTimeoutOperationPending = 0;
#endif
        internal void Enqueue(OperationRequest operationRequest, Action<OperationResponse> onOperationResponse)
        {
#if EUN
            var operationPending = new OperationPending(operationRequest, onOperationResponse);

            if (operationRequest.IsSynchronizationRequest())
            {
                if (operationRequest.GetOperationCode() == OperationCode.VoiceChat) voiceChatOperationPendingQueue.Enqueue(operationPending);
                else syncOperationPendingQueue.Enqueue(operationPending);
            }
            else operationPendingQueue.Enqueue(operationPending);
#endif
        }

        private void Send(OperationPending operationPending)
        {
#if EUN
            var onOperationResponse = operationPending.GetCallback();
            var operationRequest = operationPending.GetOperationRequest();

            if (onOperationResponse != null) operationRequest.SetRequestId(requestId++);
            else operationRequest.SetRequestId(-1);

            var request = EzyEntityFactory.newObject();
            var data = EzyEntityFactory.newArray();
            data.add((int)operationRequest.GetOperationCode());
            data.add(operationRequest.GetParameters() == null ? null : operationRequest.GetParameters().toData());

            if (operationRequest.GetRequestId() != -1)
            {
                data.add(operationRequest.GetRequestId());

                operationRequest.SetEndTimeOut(Time.time + operationRequest.GetTimeOut());

                operationWaitingResponseDic[operationRequest.GetRequestId()] = operationPending;
            }

            request.put(Commands.Data, data);

            ezySocketObject.Send(request, operationRequest.IsReliable());

            if (operationRequest.IsSynchronizationRequest()) Debug.Log("[SEND SYNC] " + operationRequest.ToString());
            else Debug.Log("[SEND] " + operationRequest.ToString());
#endif
        }

        private void Update()
        {
#if EUN
            serverTimeStamp += Time.deltaTime * 1000;

            if (checkTimeoutOperationPending < Time.time)
            {
                checkTimeoutOperationPending = Time.time + 0.1f;

                if (operationWaitingResponseDic.Count != 0)
                {
                    var ezyArrayLst = new List<EzyArray>();

                    foreach (var operationPendingPair in operationWaitingResponseDic)
                    {
                        var operationPending = operationPendingPair.Value;
                        var operationRequest = operationPending.GetOperationRequest();

                        if (operationRequest.GetEndTimeOut() < Time.time)
                        {
                            var ezyData = EzyEntityFactory.newArray();

                            ezyData.add((int)ReturnCode.OperationTimeout);
                            ezyData.add((string)null);
                            ezyData.add(operationRequest.GetRequestId());

                            ezyArrayLst.Add(ezyData);
                        }
                    }

                    if (ezyArrayLst.Count == 0)
                    {
                        foreach (var ezyData in ezyArrayLst)
                        {
                            OnResponseHandler(ezyData);
                        }
                    }
                }
            }

            if (nextSendMsgTimer < Time.time)
            {
                if (operationPendingQueue.Count != 0)
                {
                    nextSendMsgTimer = Time.time + perMsgTimer;
                    Send(operationPendingQueue.Dequeue());
                }
            }

            if (room != null && room.RoomPlayerLst.Count > 1)
            {
                if (nextSendSyncMsgTimer < Time.time)
                {
                    foreach (var view in ezyViewLst)
                    {
                        if (view)
                        {
                            foreach (var behaviour in view.ezyBehaviourLst)
                            {
                                if (behaviour)
                                {
                                    if (behaviour.gameObject.activeInHierarchy)
                                    {
                                        var objectData = behaviour.GetSynchronizationData();
                                        if (objectData != null) SynchronizationDataGameObjectRoom(behaviour.ezyView.RoomGameObject.ObjectId, objectData);
                                    }
                                }
                            }
                        }
                    }

                    if (syncOperationPendingQueue.Count != 0)
                    {
                        nextSendSyncMsgTimer = Time.time + perSyncMsgTimer;
                        Send(syncOperationPendingQueue.Dequeue());
                    }
                }

                if (nextSendVoiceChatMsgTimer < Time.time)
                {
                    foreach (var view in ezyViewLst)
                    {
                        if (view)
                        {
                            foreach (var behaviour in view.ezyVoiceChatBehaviourLst)
                            {
                                if (behaviour)
                                {
                                    if (behaviour.gameObject.activeInHierarchy)
                                    {
                                        var objectData = behaviour.GetSynchronizationData();
                                        if (objectData != null) VoiceChatRoom(behaviour.ezyView.RoomGameObject.ObjectId, objectData);
                                    }
                                }
                            }
                        }
                    }

                    if (voiceChatOperationPendingQueue.Count != 0)
                    {
                        nextSendVoiceChatMsgTimer = Time.time + perVoiceChatMsgTimer;
                        Send(voiceChatOperationPendingQueue.Dequeue());
                    }
                }
            }  
#endif
        }
    }
}