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

    public partial class NetworkingPeer
    {
        internal int lobbyId = -1;

        internal Room room;

        internal int playerId;

        internal Dictionary<int, EzyView> ezyViewDic;

        internal long tsServerTime => (long)serverTimeStamp;

        internal bool isConnected;

        internal List<RoomGameObject> getListGameObjectNeedCreate()
        {
            if (room == null) return null;

            var removeLst = new List<int>();
            var createLst = new List<int>();

            foreach (var c in room.GameObjectDic)
            {
                if (ezyViewDic.ContainsKey(c.Key))
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

            foreach (var c in ezyViewDic)
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
                    ezyViewDic.Remove(c);
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

        internal void SetSendRate(int sendRate, int sendRateSynchronizationData, int sendRateVoiceChat)
        {
            if (sendRate < 1) sendRate = 1;
            if (sendRateSynchronizationData < 1) sendRateSynchronizationData = 1;
            if (sendRateVoiceChat < 1) sendRateVoiceChat = 1;

            perMsgTimer = 1f / sendRate;
            perSyncMsgTimer = 1f / sendRateSynchronizationData;
            perVoiceChatMsgTimer = 1f / sendRateVoiceChat;
        }

        internal void Connect(string username, string password, ICustomData data)
        {
            var ezyServerSettings = EzyNetwork.ezyServerSettings;
            if (ezyServerSettings == null) throw new NullReferenceException("Null Ezy Server Settings, please find it now");

            if (EzyNetwork.Mode == Config.EzyServerSettings.Mode.OfflineMode)
            {
                OnConnectionSuccessHandler();

                var obj = new CustomArray.Builder()
                .Add(null)
                .Add(null)
                .Add(
                    new CustomArray.Builder()
                    .Add(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                    .Build())
                .Build();

                OnAppAccessHandler(obj);
            }
            else
            {
#if !UNITY_EDITOR && UNITY_WEBGL
                ezySocketObject.Connect(username, password, data, ezyServerSettings.webSocketHost, 0, 0);
#else
                ezySocketObject.Connect(username, password, data, ezyServerSettings.socketHost, ezyServerSettings.socketTCPPort, ezyServerSettings.socketUDPPort);
#endif
            }
        }

        internal void SyncTs(Action<SyncTsOperationResponse> onResponse)
        {
            var request = new SyncTsOperationRequest().Builder();

            if (EzyNetwork.Mode == Config.EzyServerSettings.Mode.OfflineMode)
            {
                var responseCustomHashtable = new CustomHashtable.Builder()
                    .Add(ParameterCode.Ts, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                    .Build();

                var response = new OperationResponse(request, (int)ReturnCode.Ok, string.Empty, responseCustomHashtable);

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

            if (EzyNetwork.Mode == Config.EzyServerSettings.Mode.OfflineMode)
            {
                var responseCustomHashtable = new CustomHashtable.Builder()
                    .Add(ParameterCode.Data, new CustomArray())
                    .Build();

                var response = new OperationResponse(request, (int)ReturnCode.Ok, string.Empty, responseCustomHashtable);

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

        internal void JoinOrCreateRoom(int targetExpectedCount, CustomHashtable expectedProperties, RoomOption roomOption, Action<JoinOrCreateRoomOperationResponse> onResponse)
        {
            var request = new JoinOrCreateRoomOperationRequest(targetExpectedCount, expectedProperties, roomOption).Builder();

            Enqueue(request, response =>
            {
                var joinOrCreateRoomOperationResponse = new JoinOrCreateRoomOperationResponse(response);
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

        internal void ChangeRoomInfo(CustomHashtable customHashtable, Action<ChangeRoomInfoOperationResponse> onResponse)
        {
            var request = new ChangeRoomInfoOperationRequest(customHashtable).Builder();

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

        internal void ChangePlayerCustomProperties(int playerId, CustomHashtable customPlayerProperties, Action<ChangePlayerCustomPropertiesOperationResponse> onResponse)
        {
            var request = new ChangePlayerCustomPropertiesOperationRequest(playerId, customPlayerProperties).Builder();

            Enqueue(request, response =>
            {
                var changePlayerCustomPropertiesOperationResponse = new ChangePlayerCustomPropertiesOperationResponse(response);
                onResponse?.Invoke(changePlayerCustomPropertiesOperationResponse);
            });
        }

        internal void RpcGameObjectRoom(EzyTargets targets, int objectId, int eunRPCCommand, object rpcData)
        {
            var request = new RpcGameObjectRoomOperationRequest(targets, objectId, eunRPCCommand, rpcData).Builder();

            Enqueue(request, null);
        }

        internal void RpcGameObjectRoomTo(IList<int> targetPlayerIds, int objectId, int eunRPCCommand, object rpcData)
        {
            var request = new RpcGameObjectRoomToOperationRequest(targetPlayerIds, objectId, eunRPCCommand, rpcData).Builder();

            Enqueue(request, null);
        }

        internal void CreateGameObjectRoom(string prefabPath, object initializeData, object synchronizationData, CustomHashtable customGameObjectProperties, Action<CreateGameObjectRoomOperationResponse> onResponse)
        {
            var request = new CreateGameObjectRoomOperationRequest(prefabPath, initializeData, synchronizationData, customGameObjectProperties).Builder();

            Enqueue(request, response =>
            {
                var createGameObjectRoomOperationResponse = new CreateGameObjectRoomOperationResponse(response);
                onResponse?.Invoke(createGameObjectRoomOperationResponse);
            });
        }

        internal void ChangeGameObjectCustomProperties(int objectId, CustomHashtable customGameObjectProperties, Action<ChangeGameObjectRoomOperationResponse> onResponse)
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

        internal void TransferGameObjectRoom(int objectId, int ownerId, Action<TransferGameObjectRoomOperationResponse> onResponse)
        {
            var request = new TransferGameObjectRoomOperationRequest(objectId, ownerId).Builder();

            Enqueue(request, response =>
            {
                var transferGameObjectRoomOperationResponse = new TransferGameObjectRoomOperationResponse(response);
                onResponse?.Invoke(transferGameObjectRoomOperationResponse);
            });
        }
    }
}