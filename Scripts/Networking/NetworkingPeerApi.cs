namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    using System;
    using XmobiTea.EUN.Entity;
    using System.Collections.Generic;
    using XmobiTea.EUN.Entity.Response;

#if EUN_USING_ONLINE
    using com.tvd12.ezyfoxserver.client.factory;
#endif

    using XmobiTea.EUN.Entity.Request;
    using System.Linq;

    /// <summary>
    /// In this partial it will contains all function API
    /// </summary>
    public partial class NetworkingPeer
    {
        /// <summary>
        /// The current lobby id
        /// </summary>
        internal int lobbyId = -1;

        /// <summary>
        /// The current room
        /// </summary>
        internal Room room { get; set; }

        /// <summary>
        /// The current player id in this room
        /// </summary>
        internal int playerId;

        /// <summary>
        /// Dict of eunView
        /// </summary>
        internal Dictionary<int, EUNView> eunViewDict;

        /// <summary>
        /// The server timestamp as long
        /// </summary>
        internal long tsServerTime => (long)this.serverTimestamp;

        /// <summary>
        /// Is the EUN Network connect EUN Server?
        /// </summary>
        internal bool isConnected;

        /// <summary>
        /// Get the list game object does not have agent EUNView to handle this game object room
        /// </summary>
        /// <returns></returns>
        internal List<RoomGameObject> getListGameObjectNeedCreate()
        {
            if (this.room == null) return null;

            var removeLst = new List<int>();
            var createLst = new List<int>();

            foreach (var c in this.room.gameObjectDict)
            {
                if (this.eunViewDict.ContainsKey(c.Key))
                {
                    if (c.Value == null)
                    {
                        if (!removeLst.Contains(c.Key)) removeLst.Add(c.Key);
                        if (!createLst.Contains(c.Key)) createLst.Add(c.Key);
                    }
                }
                else
                {
                    createLst.Add(c.Key);
                }
            }

            foreach (var c in this.eunViewDict)
            {
                if (!this.room.gameObjectDict.ContainsKey(c.Key))
                {
                    if (!removeLst.Contains(c.Key)) removeLst.Add(c.Key);

                    if (c.Value != null)
                    {
                        UnityEngine.GameObject.Destroy(c.Value.gameObject);
                    }
                }
            }

            if (removeLst.Count != 0)
            {
                foreach (var c in removeLst)
                {
                    this.eunViewDict.Remove(c);
                }
            }

            var roomGameObjectNeedCreateLst = new List<RoomGameObject>();

            if (createLst.Count != 0)
            {
                foreach (var c in createLst)
                {
                    roomGameObjectNeedCreateLst.Add(this.room.gameObjectDict[c]);
                }
            }

            return roomGameObjectNeedCreateLst;
        }

        /// <summary>
        /// Set the send rate, this help peer send the OperationRequest follow rate
        /// </summary>
        /// <param name="sendRate">Send rate for normal OperationRequest</param>
        /// <param name="sendRateSynchronizationData">Send rate for sync OperationRequest</param>
        /// <param name="sendRateVoiceChat">Send rate for voice chat OperationRequest</param>
        internal void setSendRate(int sendRate, int sendRateSynchronizationData, int sendRateVoiceChat)
        {
            if (sendRate < 1) sendRate = 1;
            if (sendRateSynchronizationData < 1) sendRateSynchronizationData = 1;
            if (sendRateVoiceChat < 1) sendRateVoiceChat = 1;

            this.perMsgTimer = 1f / sendRate;
            this.perSyncMsgTimer = 1f / sendRateSynchronizationData;
            this.perVoiceChatMsgTimer = 1f / sendRateVoiceChat;
        }

        /// <summary>
        /// Connect to EUNServer, the username and password in the Ezyfox Server
        /// In EUN, we use user id as user name, so you can see we use user name as user id as somewhere
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="customData"></param>
        /// <exception cref="NullReferenceException"></exception>
        internal void connect(string username, string password, IEUNData customData)
        {
            var eunServerSettings = EUNNetwork.eunServerSettings;
            if (eunServerSettings == null) throw new NullReferenceException("Null EUN Server Settings, please find it now");

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                this.onConnectionSuccessHandler();

                var obj = new EUNArray.Builder()
                .add(null)
                .add(null)
                .add(
                    new EUNArray.Builder()
                    .add(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                    .build())
                .build();

                this.onAppAccessHandler(obj);
            }
            else
            {
                if (customData == null) customData = new EUNArray();

#if !UNITY_EDITOR && UNITY_WEBGL
                this.eunSocketObject.connect(username, password, customData, eunServerSettings.webSocketHost, 0, 0);
#else
                this.eunSocketObject.connect(username, password, customData, eunServerSettings.socketHost, eunServerSettings.socketTCPPort, eunServerSettings.socketUDPPort);
#endif
            }
        }

        /// <summary>
        /// Disconnect EUNNetwork to EUNServer
        /// </summary>
        internal void disconnect()
        {
            this.eunSocketObject.disconnect();
        }

        internal void syncTs(Action<SyncTsOperationResponse> onResponse)
        {
            var request = new SyncTsOperationRequest().build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var responseEUNHashtable = new EUNHashtable.Builder()
                    .add(ParameterCode.Ts, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                    .build();

                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);
                response.setParameters(responseEUNHashtable);

                this.onSyncTsResponse(response, onResponse);
            }
            else
            {
                this.enqueue(request, response =>
                {
                    this.onSyncTsResponse(response, onResponse);
                });
            }
        }

        private void onSyncTsResponse(OperationResponse response, Action<SyncTsOperationResponse> onResponse)
        {
            var syncTsOperationResponse = new SyncTsOperationResponse(response);
            if (!syncTsOperationResponse.hasError)
            {
                this.serverTimestamp = syncTsOperationResponse.serverTimeStamp;
            }

            onResponse?.Invoke(syncTsOperationResponse);
        }

        internal void getLobbyStatsLst(int skip, int limit, Action<GetLobbyStatsLstOperationResponse> onResponse)
        {
            var request = new GetLobbyStatsLstOperationRequest(skip, limit).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var responseEUNHashtable = new EUNHashtable.Builder()
                    .add(ParameterCode.Data, new EUNArray())
                    .build();

                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);
                response.setParameters(responseEUNHashtable);

                var getLobbyStatsLstOperationResponse = new GetLobbyStatsLstOperationResponse(response);
                onResponse?.Invoke(getLobbyStatsLstOperationResponse);
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var getLobbyStatsLstOperationResponse = new GetLobbyStatsLstOperationResponse(response);
                        onResponse?.Invoke(getLobbyStatsLstOperationResponse);
                    });
                }
            }
        }

        internal void getCurrentLobbyStats(int skip, int limit, Action<GetCurrentLobbyStatsOperationResponse> onResponse)
        {
            var request = new GetCurrentLobbyStatsOperationRequest(skip, limit).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var responseEUNHashtable = new EUNHashtable.Builder()
                    .add(ParameterCode.Data, new EUNArray.Builder().add(new EUNArray()).add(new EUNArray()).build())
                    .build();

                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);
                response.setParameters(responseEUNHashtable);

                var getCurrentLobbyStatsOperationResponse = new GetCurrentLobbyStatsOperationResponse(response);
                onResponse?.Invoke(getCurrentLobbyStatsOperationResponse);
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var getCurrentLobbyStatsOperationResponse = new GetCurrentLobbyStatsOperationResponse(response);
                        onResponse?.Invoke(getCurrentLobbyStatsOperationResponse);
                    });
                }
            }
        }

        internal void joinLobby(int lobbyId, Action<JoinLobbyOperationResponse> onResponse)
        {
            var request = new JoinLobbyOperationRequest(lobbyId).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var joinLobbyOperationResponse = new JoinLobbyOperationResponse(response);
                onResponse?.Invoke(joinLobbyOperationResponse);
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var joinLobbyOperationResponse = new JoinLobbyOperationResponse(response);
                        onResponse?.Invoke(joinLobbyOperationResponse);
                    });
                }
            }
        }

        internal void leaveLobby(Action<LeaveLobbyOperationResponse> onResponse)
        {
            var request = new LeaveLobbyOperationRequest().build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var leaveLobbyOperationResponse = new LeaveLobbyOperationResponse(response);
                onResponse?.Invoke(leaveLobbyOperationResponse);
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var leaveLobbyOperationResponse = new LeaveLobbyOperationResponse(response);
                        onResponse?.Invoke(leaveLobbyOperationResponse);
                    });
                }
            }
        }

        internal void chatAll(string message, Action<ChatAllOperationResponse> onResponse)
        {
            var request = new ChatAllOperationRequest(message).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var chatAllOperationResponse = new ChatAllOperationResponse(response);
                onResponse?.Invoke(chatAllOperationResponse);

                if (this.isSubscriberChatAll)
                {
                    var eventArray = new EUNArray.Builder()
                    .add(EventCode.OnChatAll)
                    .add(
                        new EUNHashtable.Builder()
                            .add(ParameterCode.Message, new EUNArray.Builder().add(EUNNetwork.userId).add(message).build())
                            .build())
                    .build();

                    this.onEventHandler(eventArray);
                }
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var chatAllOperationResponse = new ChatAllOperationResponse(response);
                        onResponse?.Invoke(chatAllOperationResponse);
                    });
                }
            }
        }

        internal void chatLobby(string message, Action<ChatLobbyOperationResponse> onResponse)
        {
            var request = new ChatLobbyOperationRequest(message).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var chatLobbyOperationResponse = new ChatLobbyOperationResponse(response);
                onResponse?.Invoke(chatLobbyOperationResponse);

                if (this.isSubscriberChatLobby)
                {
                    var eventArray = new EUNArray.Builder()
                    .add(EventCode.OnChatLobby)
                    .add(
                        new EUNHashtable.Builder()
                            .add(ParameterCode.Message, new EUNArray.Builder().add(EUNNetwork.userId).add(message).build())
                            .build())
                    .build();

                    this.onEventHandler(eventArray);
                }
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var chatLobbyOperationResponse = new ChatLobbyOperationResponse(response);
                        onResponse?.Invoke(chatLobbyOperationResponse);
                    });
                }
            }
        }

        internal void chatRoom(string message, Action<ChatRoomOperationResponse> onResponse)
        {
            var request = new ChatRoomOperationRequest(message).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var chatRoomOperationResponse = new ChatRoomOperationResponse(response);
                onResponse?.Invoke(chatRoomOperationResponse);

                if (this.room != null)
                {
                    var eventArray = new EUNArray.Builder()
                    .add(EventCode.OnChatRoom)
                    .add(
                        new EUNHashtable.Builder()
                            .add(ParameterCode.Message, new EUNArray.Builder().add(EUNNetwork.userId).add(message).build())
                            .build())
                    .build();

                    this.onEventHandler(eventArray);
                }
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var chatRoomOperationResponse = new ChatRoomOperationResponse(response);
                        onResponse?.Invoke(chatRoomOperationResponse);
                    });
                }
            }

        }

        internal void createRoom(RoomOption roomOption, Action<CreateRoomOperationResponse> onResponse)
        {
            var request = new CreateRoomOperationRequest(roomOption).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var createRoomOperationResponse = new CreateRoomOperationResponse(response);
                onResponse?.Invoke(createRoomOperationResponse);

                if (this.room == null)
                {
                    var eventArray = new EUNArray.Builder()
                        .add(EventCode.OnJoinRoom)
                        .add(
                            new EUNHashtable.Builder()
                                .add(ParameterCode.Data, new EUNArray.Builder()
                                    .add(UnityEngine.Random.Range(0, int.MaxValue))
                                    .add(true)
                                    .add(roomOption.maxPlayer)
                                    .add(roomOption.password)
                                    .add(
                                        new EUNArray.Builder()
                                            .add(new EUNArray.Builder()
                                                    .add(1)
                                                    .add(EUNNetwork.userId)
                                                    .add(new EUNHashtable())
                                                    .build()
                                                )
                                            .build()
                                    )
                                    .add(roomOption.customRoomProperties)
                                    .add(roomOption.customRoomPropertiesForLobby)
                                    .add(true)
                                    .add(EUNNetwork.userId)
                                    .add(serverTimestamp)
                                    .add(roomOption.ttl)
                                    .build())
                                .build())
                        .build();
                    
                    this.onEventHandler(eventArray);
                }
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var createRoomOperationResponse = new CreateRoomOperationResponse(response);
                        onResponse?.Invoke(createRoomOperationResponse);
                    });
                }
            }
        }

        internal void joinOrCreateRoom(int targetExpectedCount, EUNHashtable expectedProperties, RoomOption roomOption, Action<JoinOrCreateRoomOperationResponse> onResponse)
        {
            var request = new JoinOrCreateRoomOperationRequest(targetExpectedCount, expectedProperties, roomOption).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var joinOrCreateRoomOperationResponse = new JoinOrCreateRoomOperationResponse(response);
                onResponse?.Invoke(joinOrCreateRoomOperationResponse);

                if (this.room == null)
                {
                    var eventArray = new EUNArray.Builder()
                        .add(EventCode.OnJoinRoom)
                        .add(
                            new EUNHashtable.Builder()
                                .add(ParameterCode.Data, new EUNArray.Builder()
                                    .add(UnityEngine.Random.Range(0, int.MaxValue))
                                    .add(true)
                                    .add(roomOption.maxPlayer)
                                    .add(roomOption.password)
                                    .add(
                                        new EUNArray.Builder()
                                            .add(new EUNArray.Builder()
                                                    .add(1)
                                                    .add(EUNNetwork.userId)
                                                    .add(new EUNHashtable())
                                                    .build()
                                                )
                                            .build()
                                    )
                                    .add(roomOption.customRoomProperties)
                                    .add(roomOption.customRoomPropertiesForLobby)
                                    .add(true)
                                    .add(EUNNetwork.userId)
                                    .add(serverTimestamp)
                                    .add(roomOption.ttl)
                                    .build())
                                .build())
                        .build();

                    this.onEventHandler(eventArray);
                }
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var joinOrCreateRoomOperationResponse = new JoinOrCreateRoomOperationResponse(response);
                        onResponse?.Invoke(joinOrCreateRoomOperationResponse);
                    });
                }
            }
        }

        internal void joinRandomRoom(int targetExpectedCount, EUNHashtable expectedProperties, Action<JoinRandomRoomOperationResponse> onResponse)
        {
            var request = new JoinRandomRoomOperationRequest(targetExpectedCount, expectedProperties).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.OperationInvalid);
                response.setDebugMessage(string.Empty);

                var joinOrCreateRoomOperationResponse = new JoinRandomRoomOperationResponse(response);
                onResponse?.Invoke(joinOrCreateRoomOperationResponse);
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var joinOrCreateRoomOperationResponse = new JoinRandomRoomOperationResponse(response);
                        onResponse?.Invoke(joinOrCreateRoomOperationResponse);
                    });
                }
            }
        }

        internal void joinRoom(int roomId, string password, Action<JoinRoomOperationResponse> onResponse)
        {
            var request = new JoinRoomOperationRequest(roomId, password).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.OperationInvalid);
                response.setDebugMessage(string.Empty);

                var joinRoomOperationResponse = new JoinRoomOperationResponse(response);
                onResponse?.Invoke(joinRoomOperationResponse);
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var joinRoomOperationResponse = new JoinRoomOperationResponse(response);
                        onResponse?.Invoke(joinRoomOperationResponse);
                    });
                }
            }
        }

        internal void leaveRoom(Action<LeaveRoomOperationResponse> onResponse)
        {
            var request = new LeaveRoomOperationRequest().build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var leaveRoomOperationResponse = new LeaveRoomOperationResponse(response);
                onResponse?.Invoke(leaveRoomOperationResponse);

                if (this.room != null)
                {
                    var eventArray = new EUNArray.Builder()
                        .add(EventCode.OnLeftRoom)
                        .build();

                    this.onEventHandler(eventArray);
                }
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var leaveRoomOperationResponse = new LeaveRoomOperationResponse(response);
                        onResponse?.Invoke(leaveRoomOperationResponse);
                    });
                }
            }
        }

        internal void changeLeaderClient(int leaderClientPlayerId, Action<ChangeLeaderClientOperationResponse> onResponse)
        {
            var request = new ChangeLeaderClientOperationRequest(leaderClientPlayerId).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var changeLeaderClientOperationResponse = new ChangeLeaderClientOperationResponse(response);
                onResponse?.Invoke(changeLeaderClientOperationResponse);

                var localPlayer = EUNNetwork.localPlayer;
                if (this.room != null && localPlayer != null)
                {
                    var eventArray = new EUNArray.Builder()
                        .add(EventCode.OnLeaderClientChange)
                        .add(
                            new EUNHashtable.Builder()
                                .add(ParameterCode.Data, new EUNArray.Builder().add(playerId).add(EUNNetwork.userId).add(localPlayer.customProperties).build())
                                .build())
                        .build();

                    this.onEventHandler(eventArray);
                }
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var changeLeaderClientOperationResponse = new ChangeLeaderClientOperationResponse(response);
                        onResponse?.Invoke(changeLeaderClientOperationResponse);
                    });
                }
            }
        }

        internal void changeRoomInfo(EUNHashtable eunHashtable, Action<ChangeRoomInfoOperationResponse> onResponse)
        {
            var request = new ChangeRoomInfoOperationRequest(eunHashtable).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var changeRoomInfoOperationResponse = new ChangeRoomInfoOperationResponse(response);
                onResponse?.Invoke(changeRoomInfoOperationResponse);

                if (this.room != null)
                {
                    var eventArray = new EUNArray.Builder()
                        .add(EventCode.OnRoomInfoChange)
                        .add(eunHashtable)
                        .build();
                    
                    this.onEventHandler(eventArray);
                }
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var changeRoomInfoOperationResponse = new ChangeRoomInfoOperationResponse(response);
                        onResponse?.Invoke(changeRoomInfoOperationResponse);
                    });
                }
            }
        }

        private bool isSubscriberChatAll;
        internal void subscriberChatAll(bool isSubscribe, Action<SubscriberChatAllOperationResponse> onResponse)
        {
            this.isSubscriberChatAll = isSubscribe;

            var request = new SubscriberChatAllOperationRequest(isSubscribe).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var subscriberChatAllOperationResponse = new SubscriberChatAllOperationResponse(response);
                onResponse?.Invoke(subscriberChatAllOperationResponse);
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var subscriberChatAllOperationResponse = new SubscriberChatAllOperationResponse(response);
                        onResponse?.Invoke(subscriberChatAllOperationResponse);
                    });
                }
            }
        }

        private bool isSubscriberChatLobby;
        internal void subscriberChatLobby(bool isSubscribe, Action<SubscriberChatLobbyOperationResponse> onResponse)
        {
            this.isSubscriberChatLobby = isSubscribe;

            var request = new SubscriberChatLobbyOperationRequest(isSubscribe).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var subscriberChatLobbyOperationResponse = new SubscriberChatLobbyOperationResponse(response);
                onResponse?.Invoke(subscriberChatLobbyOperationResponse);
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var subscriberChatLobbyOperationResponse = new SubscriberChatLobbyOperationResponse(response);
                        onResponse?.Invoke(subscriberChatLobbyOperationResponse);
                    });
                }
            }
        }

        internal void changePlayerCustomProperties(int playerId, EUNHashtable customPlayerProperties, Action<ChangePlayerCustomPropertiesOperationResponse> onResponse)
        {
            var request = new ChangePlayerCustomPropertiesOperationRequest(playerId, customPlayerProperties).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var changePlayerCustomPropertiesOperationResponse = new ChangePlayerCustomPropertiesOperationResponse(response);
                onResponse?.Invoke(changePlayerCustomPropertiesOperationResponse);

                if (this.room != null)
                {
                    var eventArray = new EUNArray.Builder()
                        .add(EventCode.OnPlayerCustomPropertiesChange)
                        .add(
                            new EUNHashtable.Builder()
                                .add(ParameterCode.Data, new EUNArray.Builder().add(playerId).add(customPlayerProperties).build())
                                .build())
                        .build();

                    this.onEventHandler(eventArray);
                }
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var changePlayerCustomPropertiesOperationResponse = new ChangePlayerCustomPropertiesOperationResponse(response);
                        onResponse?.Invoke(changePlayerCustomPropertiesOperationResponse);
                    });
                }
            }
        }

        internal void rpcGameObjectRoom(EUNTargets targets, int objectId, int eunRPCCommand, object rpcData, Action<RpcGameObjectRoomOperationResponse> onResponse)
        {
            var request = new RpcGameObjectRoomOperationRequest(targets, objectId, eunRPCCommand, rpcData).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var rpcGameObjectRoomOperationResponse = new RpcGameObjectRoomOperationResponse(response);
                onResponse?.Invoke(rpcGameObjectRoomOperationResponse);

                if (this.room != null && targets != EUNTargets.Others)
                {
                    var eventArray = new EUNArray.Builder()
                    .add(EventCode.OnRpcGameObject)
                    .add(
                        new EUNHashtable.Builder()
                            .add(ParameterCode.Data, new EUNArray.Builder().add(objectId).add(eunRPCCommand).add(rpcData).build())
                            .build())
                    .build();

                    this.onEventHandler(eventArray);
                }
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var rpcGameObjectRoomOperationResponse = new RpcGameObjectRoomOperationResponse(response);
                        onResponse?.Invoke(rpcGameObjectRoomOperationResponse);
                    });
                }
            }
        }

        internal void rpcGameObjectRoomTo(IList<int> targetPlayerIds, int objectId, int eunRPCCommand, object rpcData, Action<RpcGameObjectRoomToOperationResponse> onResponse)
        {
            var request = new RpcGameObjectRoomToOperationRequest(targetPlayerIds, objectId, eunRPCCommand, rpcData).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var rpcGameObjectRoomToOperationResponse = new RpcGameObjectRoomToOperationResponse(response);
                onResponse?.Invoke(rpcGameObjectRoomToOperationResponse);

                if (this.room != null && targetPlayerIds.Contains(playerId))
                {
                    var eventArray = new EUNArray.Builder()
                    .add(EventCode.OnRpcGameObject)
                    .add(
                        new EUNHashtable.Builder()
                            .add(ParameterCode.Data, new EUNArray.Builder().add(objectId).add(eunRPCCommand).add(rpcData).build())
                            .build())
                    .build();

                    this.onEventHandler(eventArray);
                }
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var rpcGameObjectRoomToOperationResponse = new RpcGameObjectRoomToOperationResponse(response);
                        onResponse?.Invoke(rpcGameObjectRoomToOperationResponse);
                    });
                }
            }
        }

        internal void createGameObjectRoom(string prefabPath, object initializeData, object synchronizationData, EUNHashtable customGameObjectProperties, Action<CreateGameObjectRoomOperationResponse> onResponse)
        {
            var request = new CreateGameObjectRoomOperationRequest(prefabPath, initializeData, synchronizationData, customGameObjectProperties).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var createGameObjectRoomOperationResponse = new CreateGameObjectRoomOperationResponse(response);
                onResponse?.Invoke(createGameObjectRoomOperationResponse);

                if (this.room != null)
                {
                    var eventArray = new EUNArray.Builder()
                        .add(EventCode.OnCreateGameObject)
                        .add(
                            new EUNHashtable.Builder()
                                .add(ParameterCode.Data, new EUNArray.Builder().add(this.eunViewDict.Keys.Max() + 1).add(playerId).add(prefabPath).add(synchronizationData).add(initializeData).add(customGameObjectProperties).build())
                                .build())
                        .build();

                    this.onEventHandler(eventArray);
                }
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var createGameObjectRoomOperationResponse = new CreateGameObjectRoomOperationResponse(response);
                        onResponse?.Invoke(createGameObjectRoomOperationResponse);
                    });
                }
            }
        }

        internal void changeGameObjectCustomProperties(int objectId, EUNHashtable customGameObjectProperties, Action<ChangeGameObjectRoomOperationResponse> onResponse)
        {
            var request = new ChangeGameObjectCustomPropertiesOperationRequest(objectId, customGameObjectProperties).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var createGameObjectRoomOperationResponse = new ChangeGameObjectRoomOperationResponse(response);
                onResponse?.Invoke(createGameObjectRoomOperationResponse);

                if (this.room != null)
                {
                    var eventArray = new EUNArray.Builder()
                        .add(EventCode.OnGameObjectCustomPropertiesChange)
                        .add(
                            new EUNHashtable.Builder()
                                .add(ParameterCode.Data, new EUNArray.Builder().add(objectId).add(customGameObjectProperties).build())
                                .build())
                        .build();

                    this.onEventHandler(eventArray);
                }
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var createGameObjectRoomOperationResponse = new ChangeGameObjectRoomOperationResponse(response);
                        onResponse?.Invoke(createGameObjectRoomOperationResponse);
                    });
                }
            }
        }

        internal void destroyGameObjectRoom(int objectId, Action<DestroyGameObjectRoomRoomOperationResponse> onResponse)
        {
            var request = new DestroyGameObjectRoomOperationRequest(objectId).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var destroyGameObjectRoomRoomOperationResponse = new DestroyGameObjectRoomRoomOperationResponse(response);
                onResponse?.Invoke(destroyGameObjectRoomRoomOperationResponse);

                if (this.room != null)
                {
                    var eventArray = new EUNArray.Builder()
                        .add(EventCode.OnDestroyGameObject)
                        .add(
                            new EUNHashtable.Builder()
                                .add(ParameterCode.Data, new EUNArray.Builder().add(objectId).build())
                                .build())
                        .build();

                    this.onEventHandler(eventArray);
                }
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var destroyGameObjectRoomRoomOperationResponse = new DestroyGameObjectRoomRoomOperationResponse(response);
                        onResponse?.Invoke(destroyGameObjectRoomRoomOperationResponse);
                    });
                }
            }
        }

        internal void synchronizationDataGameObjectRoom(int objectId, object synchronizationData, Action<SynchronizationDataGameObjectRoomOperationResponse> onResponse)
        {
            var request = new SynchronizationDataGameObjectRoomOperationRequest(objectId, synchronizationData).build();
            request.setSynchronizationRequest(true);

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var synchronizationDataGameObjectRoomOperationResponse = new SynchronizationDataGameObjectRoomOperationResponse(response);
                onResponse?.Invoke(synchronizationDataGameObjectRoomOperationResponse);

                if (this.room != null)
                {
                    var eventArray = new EUNArray.Builder()
                        .add(EventCode.OnSynchronizationDataGameObject)
                        .add(
                            new EUNHashtable.Builder()
                                .add(ParameterCode.Data, new EUNArray.Builder().add(objectId).add(synchronizationData).build())
                                .build())
                        .build();

                    this.onEventHandler(eventArray);
                }
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var synchronizationDataGameObjectRoomOperationResponse = new SynchronizationDataGameObjectRoomOperationResponse(response);
                        onResponse?.Invoke(synchronizationDataGameObjectRoomOperationResponse);
                    });
                }
            }
        }

        internal void voiceChatRoom(int objectId, object voiceChatData, Action<VoiceChatRoomOperationResponse> onResponse)
        {
            var request = new VoiceChatOperationRequest(objectId, voiceChatData).build();
            request.setSynchronizationRequest(true);

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var voiceChatRoomOperationResponse = new VoiceChatRoomOperationResponse(response);
                onResponse?.Invoke(voiceChatRoomOperationResponse);

                if (this.room != null)
                {
                    var eventArray = new EUNArray.Builder()
                        .add(EventCode.OnVoiceChat)
                        .add(
                            new EUNHashtable.Builder()
                                .add(ParameterCode.Data, new EUNArray.Builder().add(objectId).add(voiceChatData).build())
                                .build())
                        .build();

                    this.onEventHandler(eventArray);
                }
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var voiceChatRoomOperationResponse = new VoiceChatRoomOperationResponse(response);
                        onResponse?.Invoke(voiceChatRoomOperationResponse);
                    });
                }
            }
        }

        internal void transferOwnerGameObjectRoom(int objectId, int ownerId, Action<TransferOwnerGameObjectRoomOperationResponse> onResponse)
        {
            var request = new TransferOwnerGameObjectRoomOperationRequest(objectId, ownerId).build();

            if (EUNNetwork.mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var response = new OperationResponse(request.getOperationCode(), request.getRequestId());
                response.setReturnCode(ReturnCode.Ok);
                response.setDebugMessage(string.Empty);

                var transferGameObjectRoomOperationResponse = new TransferOwnerGameObjectRoomOperationResponse(response);
                onResponse?.Invoke(transferGameObjectRoomOperationResponse);

                if (this.room != null)
                {
                    var eventArray = new EUNArray.Builder()
                        .add(EventCode.OnTransferOwnerGameObject)
                        .add(
                            new EUNHashtable.Builder()
                                .add(ParameterCode.Data, new EUNArray.Builder().add(objectId).add(ownerId).build())
                                .build())
                        .build();
                    
                    this.onEventHandler(eventArray);
                }
            }
            else
            {
                if (onResponse == null)
                {
                    this.enqueue(request, null);
                }
                else
                {
                    this.enqueue(request, response =>
                    {
                        var transferGameObjectRoomOperationResponse = new TransferOwnerGameObjectRoomOperationResponse(response);
                        onResponse?.Invoke(transferGameObjectRoomOperationResponse);
                    });
                }
            }
        }

    }

}
