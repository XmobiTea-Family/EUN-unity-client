namespace XmobiTea.EUN.Networking
{
#if EUN_USING_ONLINE
    using com.tvd12.ezyfoxserver.client.factory;
#endif

    using XmobiTea.EUN.Bride;
#if !UNITY_EDITOR && UNITY_WEBGL
    using XmobiTea.EUN.Bride.WebSocket;
#else
    using XmobiTea.EUN.Bride.Socket;
#endif
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    using System;
    using System.Collections.Generic;

    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Logger;

    /// <summary>
    /// The peer for EUNNetwork
    /// In this partial, it will handle queue, sort all request from EUNNetwork, send request and receive response
    /// Find and send request or receive response via websocket or socket
    /// </summary>
    public partial class NetworkingPeer
    {
        /// <summary>
        /// The operation pending, my think is add to queue and pending wait to send EUN Server
        /// </summary>
        class OperationPending
        {
            /// <summary>
            /// The operation request need send
            /// </summary>
            private OperationRequest operationRequest;

            private OperationResponse operationResponse;

            /// <summary>
            /// the callback if receive the operation response from EUN Server
            /// </summary>
            private Action<OperationResponse> onOperationResponse;

            private float timeout;

            private float firstSend;
            private float secondsSend;

            /// <summary>
            /// Constructor for OperationPending
            /// </summary>
            /// <param name="operationRequest"></param>
            /// <param name="onOperationResponse"></param>
            public OperationPending(OperationRequest operationRequest, Action<OperationResponse> onOperationResponse)
            {
                this.operationRequest = operationRequest;
                this.onOperationResponse = onOperationResponse;

                this.timeout = UnityEngine.Time.realtimeSinceStartup + operationRequest.getTimeout();
                this.firstSend = 0;
                this.secondsSend = 0;
            }

            public void onSend()
            {
                this.firstSend = UnityEngine.Time.realtimeSinceStartup;
                this.timeout = this.firstSend + this.operationRequest.getTimeout();
            }

            public void onRecv()
            {
                this.secondsSend = UnityEngine.Time.realtimeSinceStartup;
            }

            public float getExecuteTimerInMs()
            {
                return (this.secondsSend - this.firstSend) * 1000;
            }

            public bool isTimeout()
            {
                return this.timeout < UnityEngine.Time.realtimeSinceStartup;
            }

            public OperationRequest getOperationRequest()
            {
                return this.operationRequest;
            }

            public OperationResponse getOperationResponse()
            {
                return this.operationResponse;
            }

            public void setOperationResponse(OperationResponse operationResponse)
            {
                this.operationResponse = operationResponse;
            }

            public bool HasCallback()
            {
                return this.onOperationResponse != null;
            }

            public void Invoke()
            {
                this.onOperationResponse?.Invoke(this.operationResponse);
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
        private Dictionary<int, OperationPending> operationWaitingResponseDict;

        /// <summary>
        /// The server timestamp of EUN Server
        /// </summary>
        private double serverTimestamp;

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
        internal void initPeer()
        {
            this.serverTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if (this.operationWaitingResponseDict == null) this.operationWaitingResponseDict = new Dictionary<int, OperationPending>();
            if (this.operationPendingQueue == null) this.operationPendingQueue = new Queue<OperationPending>();
            if (this.syncOperationPendingQueue == null) this.syncOperationPendingQueue = new Queue<OperationPending>();
            if (this.voiceChatOperationPendingQueue == null) this.voiceChatOperationPendingQueue = new Queue<OperationPending>();

            if (this.serverEventHandlerDict == null) this.serverEventHandlerDict = new Dictionary<int, IServerEventHandler>();
            if (this.eunViewLst == null) this.eunViewLst = new List<EUNView>();
            if (this.eunManagerBehaviourLst == null) this.eunManagerBehaviourLst = new List<IEUNManagerBehaviour>();
            if (this.eunViewDict == null) this.eunViewDict = new Dictionary<int, EUNView>();

            if (this.eventHandlerPending == null) this.eventHandlerPending = new List<OperationEvent>();
            if (this.responseHandlerPending == null) this.responseHandlerPending = new List<OperationPending>();

            this.initSendRate();

            this.initEUNSocketObject();

            this.subscriberHandler();

            this.subscriberServerEventHandler();
        }

        /// <summary>
        /// Init send rate
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        private void initSendRate()
        {
            var eunServerSettings = EUNNetwork.eunServerSettings;
            if (eunServerSettings == null) throw new NullReferenceException("Null EUN Server Settings, please find it now");

            var sendRate = eunServerSettings.sendRate;
            var sendRateSynchronizationData = eunServerSettings.sendRateSynchronizationData;
            var sendRateVoiceChat = eunServerSettings.sendRateVoiceChat;

            this.setSendRate(sendRate, sendRateSynchronizationData, sendRateVoiceChat);
        }

        /// <summary>
        /// Init eunSocketObject
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        private void initEUNSocketObject()
        {
            var eunServerSettings = EUNNetwork.eunServerSettings;

            if (eunServerSettings == null) throw new NullReferenceException("Where is EUN Server Settings");

#if !UNITY_EDITOR && UNITY_WEBGL
            this.eunSocketObject = new WebSocketEUNSocketObject();
#else
            this.eunSocketObject = new SocketEUNSocketObject();
#endif

            var zoneName = eunServerSettings.zoneName;
            var appName = eunServerSettings.appName;

            this.eunSocketObject.init(zoneName, appName);
        }

        /// <summary>
        /// Subscriber ezy handler
        /// </summary>
        private void subscriberHandler()
        {
            this.eunSocketObject.subscriberConnectionSuccessHandler(this.onConnectionSuccessHandler);
            this.eunSocketObject.subscriberConnectionFailureHandler(this.onConnectionFailureHandler);
            this.eunSocketObject.subscriberDisconnectionHandler(this.onDisconnectionHandler);
            this.eunSocketObject.subscriberLoginErrorHandler(this.onLoginErrorHandler);
            this.eunSocketObject.subscriberAppAccessHandler(this.onAppAccessHandler);
            this.eunSocketObject.subscriberResponseHandler(this.onResponseHandler);
            this.eunSocketObject.subscriberEventHandler(this.onEventHandler);
        }

        /// <summary>
        /// this will call every scene loaded to find room game object not create game object
        /// </summary>
        public void onSceneLoaded()
        {
            var roomGameObjectNeedCreateLst = this.getListGameObjectNeedCreate();

            if (roomGameObjectNeedCreateLst != null && roomGameObjectNeedCreateLst.Count != 0)
            {
                for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                {
                    var behaviour = eunManagerBehaviourLst[i];
                    if (behaviour != null)
                    {
                        foreach (var roomGameObject in roomGameObjectNeedCreateLst)
                        {
                            var view = behaviour.onEUNViewNeedCreate(roomGameObject);
                            if (view != null)
                            {
                                view.init(roomGameObject);
                                this.eunViewDict[view.roomGameObject.objectId] = view;
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
        internal void enqueue(OperationRequest operationRequest, Action<OperationResponse> onOperationResponse)
        {
            var operationPending = new OperationPending(operationRequest, onOperationResponse);

            if (operationRequest.isSynchronizationRequest())
            {
                if (operationRequest.getOperationCode() == OperationCode.VoiceChat) lock (this.voiceChatOperationPendingQueue) this.voiceChatOperationPendingQueue.Enqueue(operationPending);
                else lock (this.syncOperationPendingQueue) this.syncOperationPendingQueue.Enqueue(operationPending);
            }
            else lock (this.operationPendingQueue) this.operationPendingQueue.Enqueue(operationPending);
        }

        /// <summary>
        /// Send a OperationPending
        /// </summary>
        /// <param name="operationPending">the operation pending</param>
        private void send(OperationPending operationPending)
        {
            operationPending.onSend();

            var operationRequest = operationPending.getOperationRequest();

            if (operationPending.HasCallback()) operationRequest.setRequestId(currentRequestId++);
            else operationRequest.setRequestId(-1);
#if EUN_USING_ONLINE
            var request = EzyEntityFactory.newObject();
            var data = EzyEntityFactory.newArray();
            data.add((int)operationRequest.getOperationCode());
            data.add(operationRequest.getParameters() == null ? null : operationRequest.getParameters().toEzyData());

            if (operationRequest.getRequestId() != -1)
            {
                data.add(operationRequest.getRequestId());

                lock (this.operationWaitingResponseDict)
                {
                    this.operationWaitingResponseDict[operationRequest.getRequestId()] = operationPending;
                }
            }

            request.put(Commands.Data, data);

            this.eunSocketObject.send(request, operationRequest.isReliable());
#endif

            if (operationRequest.isSynchronizationRequest()) EUNDebug.log("[SEND SYNC] " + operationRequest.ToString());
            else EUNDebug.log("[SEND] " + operationRequest.ToString());

        }

        public virtual void service()
        {
            this.serviceOnMainThread();

            this.eunSocketObject?.service();

            this.serverTimestamp += UnityEngine.Time.deltaTime * 1000;

            // check the timeout of request has sent if this not receive any response
            if (this.checkTimeoutOperationPending < UnityEngine.Time.time)
            {
                this.checkTimeoutOperationPending = UnityEngine.Time.time + 0.1f;

                var eunArrayResponseLst = new List<EUNArray>();

                lock (this.operationWaitingResponseDict)
                {
                    if (this.operationWaitingResponseDict.Count != 0)
                    {
                        foreach (var operationPendingPair in this.operationWaitingResponseDict)
                        {
                            var operationPending = operationPendingPair.Value;

                            if (operationPending.isTimeout())
                            {
                                var eunArray = new EUNArray();

                                eunArray.add((int)ReturnCode.OperationTimeout);
                                eunArray.add((string)null);
                                eunArray.add(operationPending.getOperationRequest().getRequestId());

                                eunArrayResponseLst.Add(eunArray);
                            }
                        }
                    }
                }

                if (eunArrayResponseLst.Count == 0)
                {
                    foreach (var eunArray in eunArrayResponseLst)
                    {
                        this.onResponseHandler(eunArray);
                    }
                }
            }

            // check and send the normal OperationPending
            if (this.nextSendMsgTimer < UnityEngine.Time.time)
            {
                OperationPending operationPending = null;
                lock (this.operationPendingQueue)
                {
                    if (this.operationPendingQueue.Count != 0)
                    {
                        this.nextSendMsgTimer = UnityEngine.Time.time + this.perMsgTimer;
                        operationPending = this.operationPendingQueue.Dequeue();
                    }
                }
                if (operationPending != null) this.send(operationPending);
            }

            if (room != null && room.roomPlayerLst.Count > 1)
            {
                // get and send the sync OperationRequest
                if (this.nextSendSyncMsgTimer < UnityEngine.Time.time)
                {
                    foreach (var view in this.eunViewLst)
                    {
                        if (view)
                        {
                            foreach (var behaviour in view.eunBehaviourLst)
                            {
                                if (behaviour)
                                {
                                    if (behaviour.gameObject.activeInHierarchy)
                                    {
                                        var objectData = behaviour.getSynchronizationData();
                                        if (objectData != null) this.synchronizationDataGameObjectRoom(behaviour.eunView.roomGameObject.objectId, objectData, null);
                                    }
                                }
                            }
                        }
                    }

                    OperationPending operationPending = null;
                    lock (this.syncOperationPendingQueue)
                    {
                        if (this.syncOperationPendingQueue.Count != 0)
                        {
                            this.nextSendSyncMsgTimer = UnityEngine.Time.time + this.perSyncMsgTimer;
                            operationPending = this.syncOperationPendingQueue.Dequeue();
                        }
                    }
                    if (operationPending != null) this.send(operationPending);
                }

                // get and send the voice chat OperationRequest
                if (this.nextSendVoiceChatMsgTimer < UnityEngine.Time.time)
                {
                    foreach (var view in this.eunViewLst)
                    {
                        if (view)
                        {
                            foreach (var behaviour in view.eunVoiceChatBehaviourLst)
                            {
                                if (behaviour != null)
                                {
                                    if (behaviour.gameObject.activeInHierarchy)
                                    {
                                        var objectData = behaviour.getSynchronizationData();
                                        if (objectData != null) this.voiceChatRoom(behaviour.eunView.roomGameObject.objectId, objectData, null);
                                    }
                                }
                            }
                        }
                    }

                    OperationPending operationPending = null;
                    lock (this.voiceChatOperationPendingQueue)
                    {
                        if (this.voiceChatOperationPendingQueue.Count != 0)
                        {
                            this.nextSendVoiceChatMsgTimer = UnityEngine.Time.time + this.perVoiceChatMsgTimer;
                            operationPending = this.voiceChatOperationPendingQueue.Dequeue();
                        }
                    }
                    if (operationPending != null) this.send(operationPending);
                }
            }
        }

    }

}
