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
    using XmobiTea.EUN.Logger;

    /// <summary>
    /// The peer for EUNNetwork
    /// In this partial, it will handle queue, sort all request from EUNNetwork, send request and receive response
    /// Find and send request or receive response via websocket or socket
    /// </summary>
    public partial class NetworkingPeer : MonoBehaviour
    {
        /// <summary>
        /// The operation pending, my think is add to queue and pending wait to send EUN Server
        /// </summary>
        public struct OperationPending
        {
            /// <summary>
            /// The operation request need send
            /// </summary>
            private OperationRequest operationRequest;

            /// <summary>
            /// the callback if receive the operation response from EUN Server
            /// </summary>
            private Action<OperationResponse> onOperationResponse;

            /// <summary>
            /// Constructor for OperationPending
            /// </summary>
            /// <param name="operationRequest"></param>
            /// <param name="onOperationResponse"></param>
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

        /// <summary>
        /// if client need use websocket (if is WebGL) or socket (other remain)
        /// </summary>
        private IEUNSocketObject eunSocketObject;

        /// <summary>
        /// The request let EUN Server known what is this OperationRequest
        /// </summary>
        private int currentRequestId;

        /// <summary>
        /// The queue of normal OperationRequest does not sending
        /// </summary>
        private Queue<OperationPending> operationPendingQueue;

        /// <summary>
        /// The queue of sync data object OperationRequest does not sending
        /// </summary>
        private Queue<OperationPending> syncOperationPendingQueue;

        /// <summary>
        /// The queue of voice chat data object OperationRequest does not sending
        /// </summary>
        private Queue<OperationPending> voiceChatOperationPendingQueue;

        /// <summary>
        /// The dict for OperationPending has sent to EUN Server and waiting for response
        /// </summary>
        private Dictionary<int, OperationPending> operationWaitingResponseDic;

        /// <summary>
        /// The server timestamp of EUN Server
        /// </summary>
        private double serverTimeStamp;

        /// <summary>
        /// The seconds timer to send one normal OperationRequest
        /// </summary>
        private float perMsgTimer;

        /// <summary>
        /// The seconds timer to send one sync OperationRequest
        /// </summary>
        private float perSyncMsgTimer;

        /// <summary>
        /// The seconds timer to send one voice chat OperationRequest
        /// </summary>
        private float perVoiceChatMsgTimer;

        /// <summary>
        /// The next timer to send next normal OperationRequest in waiting queue
        /// </summary>
        private float nextSendMsgTimer;

        /// <summary>
        /// The next timer to send next sync OperationRequest in waiting queue
        /// </summary>
        private float nextSendSyncMsgTimer;

        /// <summary>
        /// The next timer to send next voice chat OperationRequest in waiting queue
        /// </summary>
        private float nextSendVoiceChatMsgTimer;

        /// <summary>
        /// Init peer, like constructor
        /// </summary>
        internal void InitPeer()
        {
            serverTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if (operationWaitingResponseDic == null) operationWaitingResponseDic = new Dictionary<int, OperationPending>();
            if (operationPendingQueue == null) operationPendingQueue = new Queue<OperationPending>();
            if (syncOperationPendingQueue == null) syncOperationPendingQueue = new Queue<OperationPending>();
            if (voiceChatOperationPendingQueue == null) voiceChatOperationPendingQueue = new Queue<OperationPending>();

            if (serverEventHandlerDic == null) serverEventHandlerDic = new Dictionary<int, IServerEventHandler>();
            if (eunViewLst == null) eunViewLst = new List<EUNView>();
            if (eunManagerBehaviourLst == null) eunManagerBehaviourLst = new List<IEUNManagerBehaviour>();
            if (eunViewDic == null) eunViewDic = new Dictionary<int, EUNView>();

            InitSendRate();

            InitEUNSocketObject();

            SubscriberHandler();

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;

            SubscriberServerEventHandler();
        }

        /// <summary>
        /// Init send rate
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        private void InitSendRate()
        {
            var eunServerSettings = EUNNetwork.eunServerSettings;
            if (eunServerSettings == null) throw new NullReferenceException("Null EUN Server Settings, please find it now");

            var sendRate = eunServerSettings.sendRate;
            var sendRateSynchronizationData = eunServerSettings.sendRateSynchronizationData;
            var sendRateVoiceChat = eunServerSettings.sendRateVoiceChat;

            SetSendRate(sendRate, sendRateSynchronizationData, sendRateVoiceChat);
        }

        /// <summary>
        /// Init eunSocketObject
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
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

        /// <summary>
        /// Subscriber ezy handler
        /// </summary>
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

        /// <summary>
        /// this will call every scene loaded to find room game object not create game object
        /// </summary>
        /// <param name="scene">the current scene</param>
        /// <param name="loadSceneMode">the load scene mode</param>
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
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null)
                    {
                        for (var j = 0; j < roomGameObjectNeedCreateLst.Count; j++)
                        {
                            var roomGameObject = roomGameObjectNeedCreateLst[j];

                            var view = behaviour.OnEUNViewNeedCreate(roomGameObject);
                            if (view != null)
                            {
                                view.Init(roomGameObject);
                            }
                        }
                    }
                }
            }
        }

        float checkTimeoutOperationPending = 0;

        /// <summary>
        /// Enqueue the normal request to queue and waiting to send this OperationRequest to EUN Server via send rate
        /// </summary>
        /// <param name="operationRequest"></param>
        /// <param name="onOperationResponse"></param>
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

        /// <summary>
        /// Send a OperationPending
        /// </summary>
        /// <param name="operationPending">the operation pending</param>
        private void Send(OperationPending operationPending)
        {
            var onOperationResponse = operationPending.GetCallback();
            var operationRequest = operationPending.GetOperationRequest();

            if (onOperationResponse != null) operationRequest.SetRequestId(currentRequestId++);
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

            if (operationRequest.IsSynchronizationRequest()) EUNDebug.Log("[SEND SYNC] " + operationRequest.ToString());
            else EUNDebug.Log("[SEND] " + operationRequest.ToString());

        }

        private void Update()
        {
            serverTimeStamp += Time.deltaTime * 1000;

            // check the timeout of request has sent if this not receive any response
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
                        for (var i = 0; i < eunArrayLst.Count; i++)
                        {
                            var eunArray = eunArrayLst[i];

                            OnResponseHandler(eunArray);
                        }
                    }
                }
            }

            // check and send the normal OperationPending
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
                // get and send the sync OperationRequest
                if (nextSendSyncMsgTimer < Time.time)
                {
                    for (var i = 0; i < eunViewLst.Count; i++)
                    {
                        var view = eunViewLst[i];

                        if (view)
                        {
                            var eunBehaviourLst = view.eunBehaviourLst;

                            for (var j = 0; j < eunBehaviourLst.Count; j++)
                            {
                                var behaviour = eunBehaviourLst[j];

                                if (behaviour)
                                {
                                    if (behaviour.gameObject.activeInHierarchy)
                                    {
                                        var objectData = behaviour.GetSynchronizationData();
                                        if (objectData != null) SynchronizationDataGameObjectRoom(view.RoomGameObject.ObjectId, objectData);
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

                // get and send the voice chat OperationRequest
                if (nextSendVoiceChatMsgTimer < Time.time)
                {
                    for (var i = 0; i < eunViewLst.Count; i++)
                    {
                        var view = eunViewLst[i];

                        if (view)
                        {
                            var eunVoiceChatBehaviourLst = view.eunVoiceChatBehaviourLst;

                            for (var j = 0; j < eunVoiceChatBehaviourLst.Count; j++)
                            {
                                var behaviour = eunVoiceChatBehaviourLst[j];

                                if (behaviour)
                                {
                                    if (behaviour.gameObject.activeInHierarchy)
                                    {
                                        var objectData = behaviour.GetSynchronizationData();
                                        if (objectData != null) VoiceChatRoom(view.RoomGameObject.ObjectId, objectData);
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
