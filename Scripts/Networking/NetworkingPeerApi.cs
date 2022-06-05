namespace XmobiTea.EUN.Networking
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    using System;
    using XmobiTea.EUN.Entity;
    using System.Collections.Generic;
    using XmobiTea.EUN.Entity.Response;

#if EUN
    using com.tvd12.ezyfoxserver.client.factory;
#endif

    using XmobiTea.EUN.Entity.Request;

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
        internal Room room;

        /// <summary>
        /// The current player id in this room
        /// </summary>
        internal int playerId;

        /// <summary>
        /// Dict of eunView
        /// </summary>
        internal Dictionary<int, EUNView> eunViewDic { get; private set; }

        /// <summary>
        /// The server timestamp as long
        /// </summary>
        internal long tsServerTime => (long)serverTimeStamp;

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
            if (room == null) return null;

            var removeLst = new List<int>();
            var createLst = new List<int>();

            foreach (var c in room.GameObjectDic)
            {
                if (eunViewDic.ContainsKey(c.Key))
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

            foreach (var c in eunViewDic)
            {
                if (!room.GameObjectDic.ContainsKey(c.Key))
                {
                    if (!removeLst.Contains(c.Key)) removeLst.Add(c.Key);

                    if (c.Value != null)
                    {
                        Destroy(c.Value.gameObject);
                    }
                }
            }

            if (removeLst.Count != 0)
            {
                foreach (var c in removeLst)
                {
                    eunViewDic.Remove(c);
                }
            }

            var roomGameObjectNeedCreateLst = new List<RoomGameObject>();

            if (createLst.Count != 0)
            {
                foreach (var c in createLst)
                {
                    roomGameObjectNeedCreateLst.Add(room.GameObjectDic[c]);
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
        internal void SetSendRate(int sendRate, int sendRateSynchronizationData, int sendRateVoiceChat)
        {
            if (sendRate < 1) sendRate = 1;
            if (sendRateSynchronizationData < 1) sendRateSynchronizationData = 1;
            if (sendRateVoiceChat < 1) sendRateVoiceChat = 1;

            perMsgTimer = 1f / sendRate;
            perSyncMsgTimer = 1f / sendRateSynchronizationData;
            perVoiceChatMsgTimer = 1f / sendRateVoiceChat;
        }

        /// <summary>
        /// Connect to EUNServer, the username and password in the Ezyfox Server
        /// In EUN, we use user id as user name, so you can see we use user name as user id as somewhere
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="customData"></param>
        /// <exception cref="NullReferenceException"></exception>
        internal void Connect(string username, string password, IEUNData customData)
        {
            var eunServerSettings = EUNNetwork.eunServerSettings;
            if (eunServerSettings == null) throw new NullReferenceException("Null EUN Server Settings, please find it now");

            if (EUNNetwork.Mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                OnConnectionSuccessHandler();

                var obj = new EUNArray.Builder()
                .Add(null)
                .Add(null)
                .Add(
                    new EUNArray.Builder()
                    .Add(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                    .Build())
                .Build();

                OnAppAccessHandler(obj);
            }
            else
            {
                if (customData == null) customData = new EUNArray();

#if !UNITY_EDITOR && UNITY_WEBGL
                eunSocketObject.Connect(username, password, customData, eunServerSettings.webSocketHost, 0, 0);
#else
                eunSocketObject.Connect(username, password, customData, eunServerSettings.socketHost, eunServerSettings.socketTCPPort, eunServerSettings.socketUDPPort);
#endif
            }
        }

        /// <summary>
        /// Disconnect EUNNetwork to EUNServer
        /// </summary>
        internal void Disconnect()
        {
            eunSocketObject.Disconnect();
        }

        internal void SyncTs(Action<SyncTsOperationResponse> onResponse)
        {
            var request = new SyncTsOperationRequest().Builder();

            if (EUNNetwork.Mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var responseEUNHashtable = new EUNHashtable.Builder()
                    .Add(ParameterCode.Ts, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                    .Build();

                var response = new OperationResponse(request, (int)ReturnCode.Ok, string.Empty, responseEUNHashtable);

                SyncTsResponse(response, onResponse);
            }
            else
            {
                Enqueue(request, response =>
                {
                    SyncTsResponse(response, onResponse);
                });
            }
        }

        private void SyncTsResponse(OperationResponse response, Action<SyncTsOperationResponse> onResponse)
        {
            var syncTsOperationResponse = new SyncTsOperationResponse(response);
            if (!syncTsOperationResponse.HasError)
            {
                serverTimeStamp = syncTsOperationResponse.ServerTimeStamp;
            }

            onResponse?.Invoke(syncTsOperationResponse);
        }

        internal void GetLobbyStatsLst(int skip, int limit, Action<GetLobbyStatsLstOperationResponse> onResponse)
        {
            var request = new GetLobbyStatsLstOperationRequest(skip, limit).Builder();

            if (EUNNetwork.Mode == Config.EUNServerSettings.Mode.OfflineMode)
            {
                var responseEUNHashtable = new EUNHashtable.Builder()
                    .Add(ParameterCode.Data, new EUNArray())
                    .Build();

                var response = new OperationResponse(request, (int)ReturnCode.Ok, string.Empty, responseEUNHashtable);

                GetLobbyStatsLstResponse(response, onResponse);
            }
            else
            {
                Enqueue(request, response =>
                {
                    GetLobbyStatsLstResponse(response, onResponse);
                });
            }
        }

        private void GetLobbyStatsLstResponse(OperationResponse response, Action<GetLobbyStatsLstOperationResponse> onResponse)
        {
            var getLobbyStatsLstOperationResponse = new GetLobbyStatsLstOperationResponse(response);
            onResponse?.Invoke(getLobbyStatsLstOperationResponse);
        }

        internal void GetCurrentLobbyStats(int skip, int limit, Action<GetCurrentLobbyStatsOperationResponse> onResponse)
        {
            var request = new GetCurrentLobbyStatsOperationRequest(skip, limit).Builder();

            Enqueue(request, response =>
            {
                var getCurrentLobbyStatsOperationResponse = new GetCurrentLobbyStatsOperationResponse(response);
                onResponse?.Invoke(getCurrentLobbyStatsOperationResponse);
            });
        }

        internal void JoinLobby(int lobbyId, Action<JoinLobbyOperationResponse> onResponse)
        {
            var request = new JoinLobbyOperationRequest(lobbyId).Builder();

            Enqueue(request, response =>
            {
                var joinLobbyOperationResponse = new JoinLobbyOperationResponse(response);

                onResponse?.Invoke(joinLobbyOperationResponse);
            });
        }

        internal void LeaveLobby(Action<LeaveLobbyOperationResponse> onResponse = null)
        {
            var request = new LeaveLobbyOperationRequest().Builder();

            Enqueue(request, response =>
            {
                var leaveLobbyOperationResponse = new LeaveLobbyOperationResponse(response);
                onResponse?.Invoke(leaveLobbyOperationResponse);
            });
        }

        internal void ChatAll(string message)
        {
            var request = new ChatAllOperationRequest(message).Builder();

            Enqueue(request, null);
        }

        internal void ChatLobby(string message)
        {
            var request = new ChatLobbyOperationRequest(message).Builder();

            Enqueue(request, null);
        }

        internal void ChatRoom(string message)
        {
            var request = new ChatRoomOperationRequest(message).Builder();

            Enqueue(request, null);
        }

        internal void CreateRoom(RoomOption roomOption, Action<CreateRoomOperationResponse> onResponse)
        {
            var request = new CreateRoomOperationRequest(roomOption).Builder();

            Enqueue(request, response =>
            {
                var createRoomOperationResponse = new CreateRoomOperationResponse(response);
                onResponse?.Invoke(createRoomOperationResponse);
            });
        }

        internal void JoinOrCreateRoom(int targetExpectedCount, EUNHashtable expectedProperties, RoomOption roomOption, Action<JoinOrCreateRoomOperationResponse> onResponse)
        {
            var request = new JoinOrCreateRoomOperationRequest(targetExpectedCount, expectedProperties, roomOption).Builder();

            Enqueue(request, response =>
            {
                var joinOrCreateRoomOperationResponse = new JoinOrCreateRoomOperationResponse(response);
                onResponse?.Invoke(joinOrCreateRoomOperationResponse);
            });
        }

        internal void JoinRandomRoom(int targetExpectedCount, EUNHashtable expectedProperties, Action<JoinRandomRoomOperationResponse> onResponse)
        {
            var request = new JoinRandomRoomOperationRequest(targetExpectedCount, expectedProperties).Builder();

            Enqueue(request, response =>
            {
                var joinOrCreateRoomOperationResponse = new JoinRandomRoomOperationResponse(response);
                onResponse?.Invoke(joinOrCreateRoomOperationResponse);
            });
        }

        internal void JoinRoom(int roomId, string password, Action<JoinRoomOperationResponse> onResponse)
        {
            var request = new JoinRoomOperationRequest(roomId, password).Builder();

            Enqueue(request, response =>
            {
                var joinRoomOperationResponse = new JoinRoomOperationResponse(response);
                onResponse?.Invoke(joinRoomOperationResponse);
            });
        }

        internal void LeaveRoom(Action<LeaveRoomOperationResponse> onResponse)
        {
            var request = new LeaveRoomOperationRequest().Builder();

            Enqueue(request, response =>
            {
                var leaveRoomOperationResponse = new LeaveRoomOperationResponse(response);
                onResponse?.Invoke(leaveRoomOperationResponse);
            });
        }

        internal void ChangeLeaderClient(int leaderClientPlayerId, Action<ChangeLeaderClientOperationResponse> onResponse)
        {
            var request = new ChangeLeaderClientOperationRequest(leaderClientPlayerId).Builder();

            Enqueue(request, response =>
            {
                var changeLeaderClientOperationResponse = new ChangeLeaderClientOperationResponse(response);
                onResponse?.Invoke(changeLeaderClientOperationResponse);
            });
        }

        internal void ChangeRoomInfo(EUNHashtable eunHashtable, Action<ChangeRoomInfoOperationResponse> onResponse)
        {
            var request = new ChangeRoomInfoOperationRequest(eunHashtable).Builder();

            Enqueue(request, response =>
            {
                var changeRoomInfoOperationResponse = new ChangeRoomInfoOperationResponse(response);
                onResponse?.Invoke(changeRoomInfoOperationResponse);
            });
        }

        internal void SubscriberChatAll(bool isSubscribe, Action<SubscriberChatAllOperationResponse> onResponse)
        {
            var request = new SubscriberChatAllOperationRequest(isSubscribe).Builder();

            Enqueue(request, response =>
            {
                var subscriberChatAllOperationResponse = new SubscriberChatAllOperationResponse(response);
                onResponse?.Invoke(subscriberChatAllOperationResponse);
            });
        }

        internal void SubscriberChatLobby(bool isSubscribe, Action<SubscriberChatLobbyOperationResponse> onResponse)
        {
            var request = new SubscriberChatLobbyOperationRequest(isSubscribe).Builder();

            Enqueue(request, response =>
            {
                var subscriberChatLobbyOperationResponse = new SubscriberChatLobbyOperationResponse(response);
                onResponse?.Invoke(subscriberChatLobbyOperationResponse);
            });
        }

        internal void ChangePlayerCustomProperties(int playerId, EUNHashtable customPlayerProperties, Action<ChangePlayerCustomPropertiesOperationResponse> onResponse)
        {
            var request = new ChangePlayerCustomPropertiesOperationRequest(playerId, customPlayerProperties).Builder();

            Enqueue(request, response =>
            {
                var changePlayerCustomPropertiesOperationResponse = new ChangePlayerCustomPropertiesOperationResponse(response);
                onResponse?.Invoke(changePlayerCustomPropertiesOperationResponse);
            });
        }

        internal void RpcGameObjectRoom(EUNTargets targets, int objectId, int eunRPCCommand, object rpcData)
        {
            var request = new RpcGameObjectRoomOperationRequest(targets, objectId, eunRPCCommand, rpcData).Builder();

            Enqueue(request, null);
        }

        internal void RpcGameObjectRoomTo(IList<int> targetPlayerIds, int objectId, int eunRPCCommand, object rpcData)
        {
            var request = new RpcGameObjectRoomToOperationRequest(targetPlayerIds, objectId, eunRPCCommand, rpcData).Builder();

            Enqueue(request, null);
        }

        internal void CreateGameObjectRoom(string prefabPath, object initializeData, object synchronizationData, EUNHashtable customGameObjectProperties, Action<CreateGameObjectRoomOperationResponse> onResponse)
        {
            var request = new CreateGameObjectRoomOperationRequest(prefabPath, initializeData, synchronizationData, customGameObjectProperties).Builder();

            Enqueue(request, response =>
            {
                var createGameObjectRoomOperationResponse = new CreateGameObjectRoomOperationResponse(response);
                onResponse?.Invoke(createGameObjectRoomOperationResponse);
            });
        }

        internal void ChangeGameObjectCustomProperties(int objectId, EUNHashtable customGameObjectProperties, Action<ChangeGameObjectRoomOperationResponse> onResponse)
        {
            var request = new ChangeGameObjectCustomPropertiesOperationRequest(objectId, customGameObjectProperties).Builder();

            Enqueue(request, response =>
            {
                var createGameObjectRoomOperationResponse = new ChangeGameObjectRoomOperationResponse(response);
                onResponse?.Invoke(createGameObjectRoomOperationResponse);
            });
        }

        internal void DestroyGameObjectRoom(int objectId, Action<DestroyGameObjectRoomRoomOperationResponse> onResponse)
        {
            var request = new DestroyGameObjectRoomOperationRequest(objectId).Builder();

            Enqueue(request, response =>
            {
                var destroyGameObjectRoomRoomOperationResponse = new DestroyGameObjectRoomRoomOperationResponse(response);
                onResponse?.Invoke(destroyGameObjectRoomRoomOperationResponse);
            });
        }

        internal void SynchronizationDataGameObjectRoom(int objectId, object synchronizationData)
        {
            var request = new SynchronizationDataGameObjectRoomOperationRequest(objectId, synchronizationData).Builder();
            request.SetSynchronizationRequest(true);

            Enqueue(request, null);
        }

        internal void VoiceChatRoom(int objectId, object voiceChatData)
        {
            var request = new VoiceChatOperationRequest(objectId, voiceChatData).Builder();
            request.SetSynchronizationRequest(true);

            Enqueue(request, null);
        }

        internal void TransferOwnerGameObjectRoom(int objectId, int ownerId, Action<TransferOwnerGameObjectRoomOperationResponse> onResponse)
        {
            var request = new TransferOwnerGameObjectRoomOperationRequest(objectId, ownerId).Builder();

            Enqueue(request, response =>
            {
                var transferGameObjectRoomOperationResponse = new TransferOwnerGameObjectRoomOperationResponse(response);
                onResponse?.Invoke(transferGameObjectRoomOperationResponse);
            });
        }
    }
}
