namespace EUN
{
#if EUN
    using com.tvd12.ezyfoxserver.client.factory;
#endif
    using EUN.Common;
    using EUN.Entity;
    using EUN.Entity.Response;

    using System;
    using System.Collections.Generic;

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
#if EUN
            peer.GetLobbyStatsLst(0, 10, onResponse);
#endif
        }

        public static void GetCurrentLobbyStats(int skip = 0, int limit = 10, Action<GetCurrentLobbyStatsOperationResponse> onResponse = null)
        {
#if EUN
            peer.GetCurrentLobbyStats(0, 10, onResponse);
#endif
        }

        public static void JoinLobby(int lobbyId, bool subscriberChat, Action<JoinLobbyOperationResponse> onResponse = null)
        {
#if EUN
            peer.JoinLobby(lobbyId, response => {
                if (response.Success) {
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
#endif
        }

        public static void JoinDefaultLobby(bool subscriberChat, Action<JoinLobbyOperationResponse> onResponse = null)
        {
#if EUN
            JoinLobby(0, subscriberChat, onResponse);
#endif
        }

        public static void LeaveLobby(Action<LeaveLobbyOperationResponse> onResponse = null)
        {
#if EUN
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
#endif
        }

        public static void ChatAll(string message)
        {
#if EUN
            peer.ChatAll(message);
#endif
        }

        public static void ChatLobby(string message)
        {
#if EUN
            peer.ChatLobby(message);
#endif
        }

        public static void ChatRoom(string message)
        {
#if EUN
            peer.ChatRoom(message);
#endif
        }

        public static void SubscriberChatAll(bool isSubscribe, Action<SubscriberChatAllOperationResponse> onResponse = null)
        {
#if EUN
            peer.SubscriberChatAll(isSubscribe, onResponse);
#endif
        }

        public static void SubscriberChatLobby(bool isSubscribe, Action<SubscriberChatLobbyOperationResponse> onResponse = null)
        {
#if EUN
            peer.SubscriberChatLobby(isSubscribe, onResponse);
#endif
        }

        public static void CreateRoom(RoomOption roomOption, Action<CreateRoomOperationResponse> onResponse = null)
        {
#if EUN
            peer.CreateRoom(roomOption, onResponse);
#endif
        }

        public static void JoinOrCreateRoom(int targetExpectedCount, CustomHashtable expectedProperties, RoomOption roomOption, Action<JoinOrCreateRoomOperationResponse> onResponse = null)
        {
#if EUN
            peer.JoinOrCreateRoom(targetExpectedCount, expectedProperties, roomOption, onResponse);
#endif
        }

        public static void JoinRoom(int roomId, string password, Action<JoinRoomOperationResponse> onResponse = null)
        {
#if EUN
            peer.JoinRoom(roomId, password, onResponse);
#endif
        }

        public static void LeaveRoom(Action<LeaveRoomOperationResponse> onResponse = null)
        {
#if EUN
            peer.LeaveRoom(onResponse);
#endif
        }

        public static void ChangeLeaderClient(int leaderClientPlayerId, Action<ChangeLeaderClientOperationResponse> onResponse = null)
        {
#if EUN
            peer.ChangeLeaderClient(leaderClientPlayerId, onResponse);
#endif
        }

        public static void ChangePlayerCustomProperties(int playerId, CustomHashtable customPlayerProperties, Action<ChangePlayerCustomPropertiesOperationResponse> onResponse = null)
        {
#if EUN
            peer.ChangePlayerCustomProperties(playerId, customPlayerProperties, onResponse);
#endif
        }

        public static void ChangeRoomInfo(CustomHashtable customHashtable, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
#if EUN
            peer.ChangeRoomInfo(customHashtable, onResponse);
#endif
        }

        public static void CreateGameObjectRoom(string prefabPath, object initializeData, object synchronizationData, CustomHashtable customGameObjectProperties, Action<CreateGameObjectRoomOperationResponse> onResponse = null)
        {
#if EUN
            peer.CreateGameObjectRoom(prefabPath, initializeData, synchronizationData, customGameObjectProperties, onResponse);
#endif
        }

        public static void ChangeGameObjectCustomProperties(int objectId, CustomHashtable customPlayerProperties, Action<ChangeGameObjectRoomOperationResponse> onResponse = null)
        {
#if EUN
            peer.ChangeGameObjectCustomProperties(objectId, customPlayerProperties, onResponse);
#endif
        }

        public static void DestroyGameObjectRoom(int objectId, Action<DestroyGameObjectRoomRoomOperationResponse> onResponse = null)
        {
#if EUN
            peer.DestroyGameObjectRoom(objectId, onResponse);
#endif
        }

        public static void TransferGameObjectRoom(int objectId, int ownerId, Action<TransferGameObjectRoomOperationResponse> onResponse = null)
        {
#if EUN
            peer.TransferGameObjectRoom(objectId, ownerId, onResponse);
#endif
        }

        public static void RpcGameObjectRoom(EzyTargets targets, int objectId, int eunRPCCommand, object rpcData)
        {
#if EUN
            if (targets == EzyTargets.OnlyMe || targets == EzyTargets.Others || (targets == EzyTargets.LeaderClient && EzyNetwork.IsLeaderClient))
            {
                var rpcData2 = EzyEntityFactory.newArray();
                rpcData2.addAll(rpcData as object[]);

                if (peer.ezyViewDic.ContainsKey(objectId))
                {
                    var view = peer.ezyViewDic[objectId];
                    if (view)
                    {
                        foreach (var behaviour in view.ezyBehaviourLst)
                        {
                            if (behaviour) behaviour.EzyRPC(eunRPCCommand, rpcData2);
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
                    var rpcData2 = EzyEntityFactory.newArray();
                    rpcData2.addAll(rpcData as object[]);

                    if (peer.ezyViewDic.ContainsKey(objectId))
                    {
                        var view = peer.ezyViewDic[objectId];
                        if (view)
                        {
                            foreach (var behaviour in view.ezyBehaviourLst)
                            {
                                if (behaviour) behaviour.EzyRPC(eunRPCCommand, rpcData2);
                            }
                        }
                    }
                }
            }
#endif
        }

        public static void RpcGameObjectRoomTo(List<int> targetPlayerIds, int objectId, int eunRPCCommand, object rpcData)
        {
#if EUN
            peer.RpcGameObjectRoomTo(targetPlayerIds, objectId, eunRPCCommand, rpcData);
#endif
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