namespace XmobiTea.EUN
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Entity.Response;

    using System;
    using System.Collections.Generic;
    using XmobiTea.EUN.Constant;

    public static partial class EzyNetwork
    {
        public static bool IsConnected => peer.isConnected;

        public static Room Room => peer.room;

        public static bool InRoom => Room != null;

        public static int LobbyId => peer.lobbyId;

        public static long ServerTimestamp => peer.tsServerTime;

        public static RoomPlayer LocalPlayer => GetRoomPlayer(PlayerId);

        public static RoomPlayer[] RoomPlayers => !InRoom ? new RoomPlayer[0] : Room.GetRoomPlayers();

        public static RoomGameObject GetRoomGameObject(int objectId) => !InRoom ? null : !Room.GameObjectDic.ContainsKey(objectId) ? null : Room.GameObjectDic[objectId];

        public static RoomPlayer LeaderClientPlayer => !InRoom ? null : Room.RoomPlayerLst.Find(x => x.UserId.Equals(Room.LeaderClientUserId));

        public static int PlayerId => peer.playerId;

        public static bool IsLeaderClient => InRoom && UserId.Equals(Room.LeaderClientUserId);

        public static RoomPlayer GetRoomPlayer(int playerId) => !InRoom ? null : Room.RoomPlayerLst.Find(x => x.PlayerId == playerId);

        //public static void SyncTs(Action<SyncTsOperationResponse> onResponse = null)
        //{
        //    peer.SyncTs(onResponse);
        //}

        public static void GetLobbyStatsLst(int skip = 0, int limit = 10, Action<GetLobbyStatsLstOperationResponse> onResponse = null)
        {
            peer.GetLobbyStatsLst(0, 10, onResponse);
        }

        public static void GetCurrentLobbyStats(int skip = 0, int limit = 10, Action<GetCurrentLobbyStatsOperationResponse> onResponse = null)
        {
            peer.GetCurrentLobbyStats(0, 10, onResponse);
        }

        public static void JoinLobby(int lobbyId, bool subscriberChat, Action<JoinLobbyOperationResponse> onResponse = null)
        {
            peer.JoinLobby(lobbyId, response => {
                if (response.Success)
                {
                    if (subscriberChat) peer.SubscriberChatLobby(subscriberChat, null);

                    peer.lobbyId = lobbyId;
                    var ezyManagerBehaviourLst = peer.ezyManagerBehaviourLst;

                    foreach (var behaviour in ezyManagerBehaviourLst)
                    {
                        if (behaviour) behaviour.OnEzyJoinLobby();
                    }
                }

                onResponse?.Invoke(response);
            });
        }

        public static void JoinDefaultLobby(bool subscriberChat, Action<JoinLobbyOperationResponse> onResponse = null)
        {
            JoinLobby(0, subscriberChat, onResponse);
        }

        public static void LeaveLobby(Action<LeaveLobbyOperationResponse> onResponse = null)
        {
            peer.LeaveLobby(response => {
                if (response.Success)
                {
                    peer.lobbyId = -1;
                    var ezyManagerBehaviourLst = peer.ezyManagerBehaviourLst;

                    foreach (var behaviour in ezyManagerBehaviourLst)
                    {
                        if (behaviour) behaviour.OnEzyLeftLobby();
                    }
                }

                onResponse?.Invoke(response);
            });
        }

        public static void ChatAll(string message)
        {
            peer.ChatAll(message);
        }

        public static void ChatLobby(string message)
        {
            peer.ChatLobby(message);
        }

        public static void ChatRoom(string message)
        {
            peer.ChatRoom(message);
        }

        public static void SubscriberChatAll(bool isSubscribe, Action<SubscriberChatAllOperationResponse> onResponse = null)
        {
            peer.SubscriberChatAll(isSubscribe, onResponse);
        }

        public static void SubscriberChatLobby(bool isSubscribe, Action<SubscriberChatLobbyOperationResponse> onResponse = null)
        {
            peer.SubscriberChatLobby(isSubscribe, onResponse);
        }

        public static void CreateRoom(RoomOption roomOption, Action<CreateRoomOperationResponse> onResponse = null)
        {
            peer.CreateRoom(roomOption, onResponse);
        }

        public static void JoinOrCreateRoom(int targetExpectedCount, CustomHashtable expectedProperties, RoomOption roomOption, Action<JoinOrCreateRoomOperationResponse> onResponse = null)
        {
            peer.JoinOrCreateRoom(targetExpectedCount, expectedProperties, roomOption, onResponse);
        }

        public static void JoinRoom(int roomId, string password, Action<JoinRoomOperationResponse> onResponse = null)
        {
            peer.JoinRoom(roomId, password, onResponse);
        }

        public static void LeaveRoom(Action<LeaveRoomOperationResponse> onResponse = null)
        {
            peer.LeaveRoom(onResponse);
        }

        public static void ChangeLeaderClient(int leaderClientPlayerId, Action<ChangeLeaderClientOperationResponse> onResponse = null)
        {
            peer.ChangeLeaderClient(leaderClientPlayerId, onResponse);
        }

        public static void ChangePlayerCustomProperties(int playerId, CustomHashtable customPlayerProperties, Action<ChangePlayerCustomPropertiesOperationResponse> onResponse = null)
        {
            peer.ChangePlayerCustomProperties(playerId, customPlayerProperties, onResponse);
        }

        public static void ChangeRoomInfo(CustomHashtable customHashtable, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            peer.ChangeRoomInfo(customHashtable, onResponse);
        }

        public static void CreateGameObjectRoom(string prefabPath, object initializeData, object synchronizationData, CustomHashtable customGameObjectProperties, Action<CreateGameObjectRoomOperationResponse> onResponse = null)
        {
            peer.CreateGameObjectRoom(prefabPath, initializeData, synchronizationData, customGameObjectProperties, onResponse);
        }

        public static void ChangeGameObjectCustomProperties(int objectId, CustomHashtable customPlayerProperties, Action<ChangeGameObjectRoomOperationResponse> onResponse = null)
        {
            peer.ChangeGameObjectCustomProperties(objectId, customPlayerProperties, onResponse);
        }

        public static void DestroyGameObjectRoom(int objectId, Action<DestroyGameObjectRoomRoomOperationResponse> onResponse = null)
        {
            peer.DestroyGameObjectRoom(objectId, onResponse);
        }

        public static void TransferGameObjectRoom(int objectId, int ownerId, Action<TransferGameObjectRoomOperationResponse> onResponse = null)
        {
            peer.TransferGameObjectRoom(objectId, ownerId, onResponse);
        }

        public static void RpcGameObjectRoom(EzyTargets targets, int objectId, int eunRPCCommand, object rpcData)
        {
            var rpcDataArray = new CustomArray.Builder().AddAll(rpcData as object[]).Build();
            
            if (targets == EzyTargets.OnlyMe || targets == EzyTargets.Others || (targets == EzyTargets.LeaderClient && EzyNetwork.IsLeaderClient))
            {
                if (peer.ezyViewDic.ContainsKey(objectId))
                {
                    var view = peer.ezyViewDic[objectId];
                    if (view)
                    {
                        foreach (var behaviour in view.ezyBehaviourLst)
                        {
                            if (behaviour) behaviour.EzyRPC(eunRPCCommand, rpcDataArray);
                        }
                    }
                }
            }

            if (EzyNetwork.RoomPlayers.Length > 1)
            {
                if (targets != EzyTargets.OnlyMe)
                {
                    peer.RpcGameObjectRoom(targets, objectId, eunRPCCommand, rpcData);
                }
            }
            else
            {
                if (targets == EzyTargets.All)
                {
                    if (peer.ezyViewDic.ContainsKey(objectId))
                    {
                        var view = peer.ezyViewDic[objectId];
                        if (view)
                        {
                            foreach (var behaviour in view.ezyBehaviourLst)
                            {
                                if (behaviour) behaviour.EzyRPC(eunRPCCommand, rpcDataArray);
                            }
                        }
                    }
                }
            }
        }

        public static void RpcGameObjectRoomTo(List<int> targetPlayerIds, int objectId, int eunRPCCommand, object rpcData)
        {
            peer.RpcGameObjectRoomTo(targetPlayerIds, objectId, eunRPCCommand, rpcData);
        }

        //public static void SynchronizationDataGameObjectRoom(int objectId, object synchronizationData)
        //{
        //    peer.SynchronizationDataGameObjectRoom(objectId, synchronizationData);
        //}

        //public static void VoiceChatRoom(int objectId, short[] voiceChatData)
        //{
        //    peer.VoiceChatRoom(objectId, voiceChatData);
        //}
    }
}