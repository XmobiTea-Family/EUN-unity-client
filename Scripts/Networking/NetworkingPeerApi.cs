namespace EUN.Networking
{
    using EUN.Common;
    using EUN.Constant;

    using System;
    using EUN.Entity;
    using System.Collections.Generic;
    using EUN.Entity.Response;
    using com.tvd12.ezyfoxserver.client.factory;

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

        internal void Connect(string userId)
        {
            var ezyServerSettings = EzyNetwork.ezyServerSettings;
            if (ezyServerSettings == null) throw new NullReferenceException("Null Ezy Server Settings, please find it now");

#if !UNITY_EDITOR && UNITY_WEBGL
            ezySocketObject.Connect(userId, ezyServerSettings.webSocketHost, 0, 0);
#else
            ezySocketObject.Connect(userId, ezyServerSettings.socketHost, ezyServerSettings.socketTCPPort, ezyServerSettings.socketUDPPort);
#endif
        }

        internal void SyncTs(Action<SyncTsOperationResponse> onResponse)
        {
            var request = new OperationRequest((int)OperationCode.SyncTs);
            Enqueue(request, response =>
            {
                var syncTsOperationResponse = new SyncTsOperationResponse(response);
                if (!syncTsOperationResponse.HasError)
                {
                    serverTimeStamp = syncTsOperationResponse.ServerTimeStamp;
                }

                onResponse?.Invoke(syncTsOperationResponse);
            });
        }

        internal void GetLobbyStatsLst(int skip, int limit, Action<GetLobbyStatsLstOperationResponse> onResponse)
        {
            var request = new OperationRequest((int)OperationCode.GetLobbyStatsLst);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.Skip, skip);
            parameters.Add(ParameterCode.Limit, limit);
            request.SetParameters(parameters);

            Enqueue(request, response =>
            {
                var getLobbyStatsLstOperationResponse = new GetLobbyStatsLstOperationResponse(response);
                onResponse?.Invoke(getLobbyStatsLstOperationResponse);
            });
        }

        internal void GetCurrentLobbyStats(int skip, int limit, Action<GetCurrentLobbyStatsOperationResponse> onResponse)
        {
            var request = new OperationRequest((int)OperationCode.GetCurrentLobbyStats);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.Skip, skip);
            parameters.Add(ParameterCode.Limit, limit);
            request.SetParameters(parameters);

            Enqueue(request, response =>
            {
                var getCurrentLobbyStatsOperationResponse = new GetCurrentLobbyStatsOperationResponse(response);
                onResponse?.Invoke(getCurrentLobbyStatsOperationResponse);
            });
        }

        internal void JoinLobby(int lobbyId, Action<JoinLobbyOperationResponse> onResponse)
        {
            var request = new OperationRequest((int)OperationCode.JoinLobby);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.LobbyId, lobbyId);
            request.SetParameters(parameters);

            Enqueue(request, response =>
            {
                var joinLobbyOperationResponse = new JoinLobbyOperationResponse(response);
                onResponse?.Invoke(joinLobbyOperationResponse);
            });
        }

        internal void LeaveLobby(Action<LeaveLobbyOperationResponse> onResponse = null)
        {
            var request = new OperationRequest((int)OperationCode.LeaveLobby);

            Enqueue(request, response =>
            {
                var leaveLobbyOperationResponse = new LeaveLobbyOperationResponse(response);
                onResponse?.Invoke(leaveLobbyOperationResponse);
            });
        }

        internal void ChatAll(string message)
        {
            var request = new OperationRequest((int)OperationCode.ChatAll, false);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.Message, message);
            request.SetParameters(parameters);

            Enqueue(request, null);
        }

        internal void ChatLobby(string message)
        {
            var request = new OperationRequest((int)OperationCode.ChatLobby, false);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.Message, message);
            request.SetParameters(parameters);

            Enqueue(request, null);
        }

        internal void ChatRoom(string message)
        {
            var request = new OperationRequest((int)OperationCode.ChatRoom, false);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.Message, message);
            request.SetParameters(parameters);

            Enqueue(request, null);
        }

        internal void CreateRoom(RoomOption roomOption, Action<CreateRoomOperationResponse> onResponse)
        {
            var request = new OperationRequest((int)OperationCode.CreateRoom);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.MaxPlayer, roomOption.MaxPlayer);
            parameters.Add(ParameterCode.CustomRoomProperties, roomOption.CustomRoomProperties);
            parameters.Add(ParameterCode.IsVisible, roomOption.IsVisible);
            parameters.Add(ParameterCode.IsOpen, roomOption.IsOpen);
            parameters.Add(ParameterCode.CustomRoomPropertiesForLobby, roomOption.CustomRoomPropertiesForLobby);
            parameters.Add(ParameterCode.Password, roomOption.Password);
            parameters.Add(ParameterCode.Ttl, roomOption.Ttl);

            request.SetParameters(parameters);

            Enqueue(request, response =>
            {
                var createRoomOperationResponse = new CreateRoomOperationResponse(response);
                onResponse?.Invoke(createRoomOperationResponse);
            });
        }

        internal void JoinOrCreateRoom(int targetExpectedCount, CustomHashtable expectedProperties, RoomOption roomOption, Action<JoinOrCreateRoomOperationResponse> onResponse)
        {
            var request = new OperationRequest((int)OperationCode.JoinOrCreateRoom);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.MaxPlayer, roomOption.MaxPlayer);
            parameters.Add(ParameterCode.TargetExpectedCount, targetExpectedCount);
            parameters.Add(ParameterCode.ExpectedProperties, expectedProperties);
            parameters.Add(ParameterCode.CustomRoomProperties, roomOption.CustomRoomProperties);
            parameters.Add(ParameterCode.IsVisible, roomOption.IsVisible);
            parameters.Add(ParameterCode.IsOpen, roomOption.IsOpen);
            parameters.Add(ParameterCode.CustomRoomPropertiesForLobby, roomOption.CustomRoomPropertiesForLobby);
            parameters.Add(ParameterCode.Password, roomOption.Password);
            parameters.Add(ParameterCode.Ttl, roomOption.Ttl);

            request.SetParameters(parameters);

            Enqueue(request, response =>
            {
                var joinOrCreateRoomOperationResponse = new JoinOrCreateRoomOperationResponse(response);
                onResponse?.Invoke(joinOrCreateRoomOperationResponse);
            });
        }

        internal void JoinRoom(int roomId, string password, Action<JoinRoomOperationResponse> onResponse)
        {
            var request = new OperationRequest((int)OperationCode.JoinRoom);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.RoomId, roomId);
            parameters.Add(ParameterCode.Password, password);

            request.SetParameters(parameters);

            Enqueue(request, response =>
            {
                var joinRoomOperationResponse = new JoinRoomOperationResponse(response);
                onResponse?.Invoke(joinRoomOperationResponse);
            });
        }

        internal void LeaveRoom(Action<LeaveRoomOperationResponse> onResponse)
        {
            var request = new OperationRequest((int)OperationCode.LeaveRoom);

            Enqueue(request, response =>
            {
                var leaveRoomOperationResponse = new LeaveRoomOperationResponse(response);
                onResponse?.Invoke(leaveRoomOperationResponse);
            });
        }

        internal void ChangeLeaderClient(int leaderClientPlayerId, Action<ChangeLeaderClientOperationResponse> onResponse)
        {
            var request = new OperationRequest((int)OperationCode.ChangeLeaderClient);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.LeaderClientPlayerId, leaderClientPlayerId);

            request.SetParameters(parameters);

            Enqueue(request, response =>
            {
                var changeLeaderClientOperationResponse = new ChangeLeaderClientOperationResponse(response);
                onResponse?.Invoke(changeLeaderClientOperationResponse);
            });
        }

        internal void ChangeRoomInfo(CustomHashtable customHashtable, Action<ChangeRoomInfoOperationResponse> onResponse)
        {
            var request = new OperationRequest((int)OperationCode.ChangeRoomInfo);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.CustomHashtable, customHashtable.toData());

            request.SetParameters(parameters);

            Enqueue(request, response =>
            {
                var changeRoomInfoOperationResponse = new ChangeRoomInfoOperationResponse(response);
                onResponse?.Invoke(changeRoomInfoOperationResponse);
            });
        }

        internal void SubscriberChatAll(bool isSubscribe, Action<SubscriberChatAllOperationResponse> onResponse)
        {
            var request = new OperationRequest((int)OperationCode.SubscriberChatAll);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.Subscribe, isSubscribe);

            request.SetParameters(parameters);

            Enqueue(request, response =>
            {
                var subscriberChatAllOperationResponse = new SubscriberChatAllOperationResponse(response);
                onResponse?.Invoke(subscriberChatAllOperationResponse);
            });
        }

        internal void SubscriberChatLobby(bool isSubscribe, Action<SubscriberChatLobbyOperationResponse> onResponse)
        {
            var request = new OperationRequest((int)OperationCode.SubscriberChatLobby);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.Subscribe, isSubscribe);

            request.SetParameters(parameters);

            Enqueue(request, response =>
            {
                var subscriberChatLobbyOperationResponse = new SubscriberChatLobbyOperationResponse(response);
                onResponse?.Invoke(subscriberChatLobbyOperationResponse);
            });
        }

        internal void ChangePlayerCustomProperties(int playerId, CustomHashtable customPlayerProperties, Action<ChangePlayerCustomPropertiesOperationResponse> onResponse)
        {
            var request = new OperationRequest((int)OperationCode.ChangePlayerCustomProperties);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.OwnerId, playerId);
            parameters.Add(ParameterCode.CustomPlayerProperties, customPlayerProperties);

            request.SetParameters(parameters);

            Enqueue(request, response =>
            {
                var changePlayerCustomPropertiesOperationResponse = new ChangePlayerCustomPropertiesOperationResponse(response);
                onResponse?.Invoke(changePlayerCustomPropertiesOperationResponse);
            });
        }

        internal void RpcGameObjectRoom(EzyTargets targets, int objectId, int eunRPCCommand, object rpcData)
        {
            var request = new OperationRequest((int)OperationCode.RpcGameObjectRoom, false);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.ObjectId, objectId);
            parameters.Add(ParameterCode.EunRPCCommand, eunRPCCommand);
            parameters.Add(ParameterCode.RpcData, rpcData);
            if (targets != EzyTargets.All) parameters.Add(ParameterCode.EzyTargets, (int)targets);

            request.SetParameters(parameters);

            Enqueue(request, null);
        }

        internal void CreateGameObjectRoom(string prefabPath, object initializeData, object synchronizationData, Action<CreateGameObjectRoomOperationResponse> onResponse)
        {
            var request = new OperationRequest((int)OperationCode.CreateGameObjectRoom);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.PrefabPath, prefabPath);
            parameters.Add(ParameterCode.InitializeData, initializeData);
            parameters.Add(ParameterCode.SynchronizationData, synchronizationData);

            request.SetParameters(parameters);

            Enqueue(request, response =>
            {
                var createGameObjectRoomOperationResponse = new CreateGameObjectRoomOperationResponse(response);
                onResponse?.Invoke(createGameObjectRoomOperationResponse);
            });
        }

        internal void DestroyGameObjectRoom(int objectId, Action<DestroyGameObjectRoomRoomOperationResponse> onResponse)
        {
            var request = new OperationRequest((int)OperationCode.DestroyGameObjectRoom);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.ObjectId, objectId);

            request.SetParameters(parameters);

            Enqueue(request, response =>
            {
                var destroyGameObjectRoomRoomOperationResponse = new DestroyGameObjectRoomRoomOperationResponse(response);
                onResponse?.Invoke(destroyGameObjectRoomRoomOperationResponse);
            });
        }

        internal void SynchronizationDataGameObjectRoom(int objectId, object synchronizationData)
        {
            var request = new OperationRequest((int)OperationCode.SynchronizationDataGameObjectRoom, false);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.ObjectId, objectId);

            if (synchronizationData is object[] synchronizationDataObjects) parameters.Add(ParameterCode.SynchronizationData, EzyEntityFactory.newArrayBuilder().appendAll(synchronizationDataObjects).build());
            else parameters.Add(ParameterCode.SynchronizationData, synchronizationData);

            request.SetParameters(parameters);
            request.SetSynchronizationRequest(true);

            Enqueue(request, null);
        }

        internal void VoiceChatRoom(int objectId, object voiceChatData)
        {
            var request = new OperationRequest((int)OperationCode.VoiceChat, false);

            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.ObjectId, objectId);
            parameters.Add(ParameterCode.SynchronizationData, voiceChatData);

            request.SetParameters(parameters);
            request.SetSynchronizationRequest(true);

            Enqueue(request, null);
        }

        internal void TransferGameObjectRoom(int objectId, int ownerId, Action<TransferGameObjectRoomOperationResponse> onResponse)
        {
            var request = new OperationRequest((int)OperationCode.TransferGameObjectRoom);
            var parameters = new CustomHashtable();
            parameters.Add(ParameterCode.ObjectId, objectId);
            parameters.Add(ParameterCode.OwnerId, ownerId);

            request.SetParameters(parameters);

            Enqueue(request, response =>
            {
                var transferGameObjectRoomOperationResponse = new TransferGameObjectRoomOperationResponse(response);
                onResponse?.Invoke(transferGameObjectRoomOperationResponse);
            });
        }
    }
}