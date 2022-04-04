namespace XmobiTea.EUN.Networking
{
#if EUN
    using com.tvd12.ezyfoxserver.client.factory;
#endif

    using XmobiTea.EUN.Bride;
    using XmobiTea.EUN.Bride.Socket;
    using XmobiTea.EUN.Bride.WebSocket;

    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    using System;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.SceneManagement;
    using XmobiTea.EUN.Entity;

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

        private IEUNSocketObject eunSocketObject;

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
            serverTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if (operationWaitingResponseDic == null) operationWaitingResponseDic = new Dictionary<int, OperationPending>();
            if (operationPendingQueue == null) operationPendingQueue = new Queue<OperationPending>();
            if (syncOperationPendingQueue == null) syncOperationPendingQueue = new Queue<OperationPending>();
            if (voiceChatOperationPendingQueue == null) voiceChatOperationPendingQueue = new Queue<OperationPending>();

            if (serverEventHandlerDic == null) serverEventHandlerDic = new Dictionary<int, IServerEventHandler>();
            if (eunViewLst == null) eunViewLst = new List<EUNView>();
            if (eunManagerBehaviourLst == null) eunManagerBehaviourLst = new List<EUNManagerBehaviour>();
            if (eunViewDic == null) eunViewDic = new Dictionary<int, EUNView>();

            InitSendRate();

            InitEUNSocketObject();

            SubscriberHandler();

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;

            SubscriberServerEventHandler();
        }

        private void InitSendRate()
        {
            var eunServerSettings = EUNNetwork.eunServerSettings;
            if (eunServerSettings == null) throw new NullReferenceException("Null EUN Server Settings, please find it now");

            var sendRate = eunServerSettings.sendRate;
            var sendRateSynchronizationData = eunServerSettings.sendRateSynchronizationData;
            var sendRateVoiceChat = eunServerSettings.sendRateVoiceChat;

            SetSendRate(sendRate, sendRateSynchronizationData, sendRateVoiceChat);
        }

        private void InitEUNSocketObject()
        {
            var eunServerSettings = EUNNetwork.eunServerSettings;

            if (eunServerSettings == null) throw new NullReferenceException("Where is EUN Server Settings");

#if !UNITY_EDITOR && UNITY_WEBGL
            eunSocketObject = gameObject.AddComponent<WebSocketEUNSocketObject>();
#else
            eunSocketObject = gameObject.AddComponent<SocketEUNSocketObject>();
#endif

            var zoneName = eunServerSettings.zoneName;
            var appName = eunServerSettings.appName;

            eunSocketObject.Init(zoneName, appName);
        }

        private void SubscriberHandler()
        {
            eunSocketObject.SubscriberConnectionSuccessHandler(OnConnectionSuccessHandler);
            eunSocketObject.SubscriberConnectionFailureHandler(OnConnectionFailureHandler);
            eunSocketObject.SubscriberDisconnectionHandler(OnDisconnectionHandler);
            eunSocketObject.SubscriberLoginErrorHandler(OnLoginErrorHandler);
            eunSocketObject.SubscriberAppAccessHandler(OnAppAccessHandler);
            eunSocketObject.SubscriberResponseHandler(OnResponseHandler);
            eunSocketObject.SubscriberEventHandler(OnEventHandler);
        }

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
                foreach (var behaviour in eunManagerBehaviourLst)
                {
                    if (behaviour)
                    {
                        foreach (var roomGameObject in roomGameObjectNeedCreateLst)
                        {
                            var view = behaviour.OnEUNViewNeedCreate(roomGameObject);
                            if (view != null)
                            {
                                view.Init(roomGameObject);
                                eunViewDic[view.RoomGameObject.ObjectId] = view;
                            }
                        }
                    }
                }
            }
        }

        float checkTimeoutOperationPending = 0;

        internal void Enqueue(OperationRequest operationRequest, Action<OperationResponse> onOperationResponse)
        {
            var operationPending = new OperationPending(operationRequest, onOperationResponse);

            if (operationRequest.IsSynchronizationRequest())
            {
                if (operationRequest.GetOperationCode() == OperationCode.VoiceChat) voiceChatOperationPendingQueue.Enqueue(operationPending);
                else syncOperationPendingQueue.Enqueue(operationPending);
            }
            else operationPendingQueue.Enqueue(operationPending);
        }

        private void Send(OperationPending operationPending)
        {
            var onOperationResponse = operationPending.GetCallback();
            var operationRequest = operationPending.GetOperationRequest();

            if (onOperationResponse != null) operationRequest.SetRequestId(requestId++);
            else operationRequest.SetRequestId(-1);
#if EUN
            var request = EzyEntityFactory.newObject();
            var data = EzyEntityFactory.newArray();
            data.add((int)operationRequest.GetOperationCode());
            data.add(operationRequest.GetParameters() == null ? null : operationRequest.GetParameters().ToEzyData());

            if (operationRequest.GetRequestId() != -1)
            {
                data.add(operationRequest.GetRequestId());

                operationRequest.SetEndTimeOut(Time.time + operationRequest.GetTimeOut());

                operationWaitingResponseDic[operationRequest.GetRequestId()] = operationPending;
            }

            request.put(Commands.Data, data);

            eunSocketObject.Send(request, operationRequest.IsReliable());
#endif

            if (operationRequest.IsSynchronizationRequest()) Debug.Log("[SEND SYNC] " + operationRequest.ToString());
            else Debug.Log("[SEND] " + operationRequest.ToString());

        }

        private void Update()
        {
            serverTimeStamp += Time.deltaTime * 1000;

            if (checkTimeoutOperationPending < Time.time)
            {
                checkTimeoutOperationPending = Time.time + 0.1f;

                if (operationWaitingResponseDic.Count != 0)
                {
                    var eunArrayLst = new List<EUNArray>();

                    foreach (var operationPendingPair in operationWaitingResponseDic)
                    {
                        var operationPending = operationPendingPair.Value;
                        var operationRequest = operationPending.GetOperationRequest();

                        if (operationRequest.GetEndTimeOut() < Time.time)
                        {
                            var eunArray = new EUNArray();

                            eunArray.Add((int)ReturnCode.OperationTimeout);
                            eunArray.Add((string)null);
                            eunArray.Add(operationRequest.GetRequestId());

                            eunArrayLst.Add(eunArray);
                        }
                    }

                    if (eunArrayLst.Count == 0)
                    {
                        foreach (var eunArray in eunArrayLst)
                        {
                            OnResponseHandler(eunArray);
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
                    foreach (var view in eunViewLst)
                    {
                        if (view)
                        {
                            foreach (var behaviour in view.eunBehaviourLst)
                            {
                                if (behaviour)
                                {
                                    if (behaviour.gameObject.activeInHierarchy)
                                    {
                                        var objectData = behaviour.GetSynchronizationData();
                                        if (objectData != null) SynchronizationDataGameObjectRoom(behaviour.eunView.RoomGameObject.ObjectId, objectData);
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
                    foreach (var view in eunViewLst)
                    {
                        if (view)
                        {
                            foreach (var behaviour in view.eunVoiceChatBehaviourLst)
                            {
                                if (behaviour)
                                {
                                    if (behaviour.gameObject.activeInHierarchy)
                                    {
                                        var objectData = behaviour.GetSynchronizationData();
                                        if (objectData != null) VoiceChatRoom(behaviour.eunView.RoomGameObject.ObjectId, objectData);
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
        }
    }
}