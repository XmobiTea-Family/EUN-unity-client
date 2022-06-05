namespace XmobiTea.EUN
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Entity.Response;

    using System;
    using System.Collections.Generic;
    using XmobiTea.EUN.Constant;

    public static partial class EUNNetwork
    {
        /// <summary>
        /// The client has connected to EUN server
        /// </summary>
        public static bool IsConnected => peer.isConnected;

        /// <summary>
        /// The room that client is inside, it null if the client is does not inroom
        /// You can join the room by request success: CreateRoom(), JoinOrCreateRoom() or JoinRandomRoom()
        /// </summary>
        public static Room Room => peer.room;

        /// <summary>
        /// The client has inroom
        /// </summary>
        public static bool InRoom => Room != null;

        /// <summary>
        /// The lobby id, where the client is in
        /// If the lobby id is -1, it mean the client is not in any lobby
        /// You can join the lobby by request success: JoinLobby(), JoinDefaultLobby()
        /// </summary>
        public static int LobbyId => peer.lobbyId;

        /// <summary>
        /// The sync current millis in the current EUN server
        /// The ServerTimestamp will sync first after NetworkingPeer.OnAppAccessHandler
        /// You can sync ServerTimestamp by request SyncTs()
        /// </summary>
        public static long ServerTimestamp => peer.tsServerTime;

        /// <summary>
        /// The client 's RoomPlayer when client in room, it will null if the client is does not inroom
        /// </summary>
        public static RoomPlayer LocalPlayer => GetRoomPlayer(PlayerId);

        /// <summary>
        /// The others player in this client room
        /// </summary>
        public static RoomPlayer[] RoomPlayers => !InRoom ? new RoomPlayer[0] : Room.GetRoomPlayers();

        /// <summary>
        /// Get the RoomGameObject by objectId
        /// </summary>
        /// <param name="objectId">Id of object</param>
        /// <returns></returns>
        public static RoomGameObject GetRoomGameObject(int objectId) => !InRoom ? null : !Room.GameObjectDic.ContainsKey(objectId) ? null : Room.GameObjectDic[objectId];

        /// <summary>
        /// You can call this is MasterClient (a client with highest permission inroom)
        /// This is the LeaderClient in this client room
        /// It may null if at this time call, the room has valid leader client
        /// </summary>
        public static RoomPlayer LeaderClientPlayer => !InRoom ? null : Room.RoomPlayerLst.Find(x => x.UserId.Equals(Room.LeaderClientUserId));

        /// <summary>
        /// It is unique id of client in this room
        /// It only valid if client inroom
        /// When client join or create room success, the EUN Server will auto provide a id in room for client.
        /// </summary>
        public static int PlayerId => peer.playerId;

        /// <summary>
        /// It true if client is Leader client
        /// </summary>
        public static bool IsLeaderClient => InRoom && UserId.Equals(Room.LeaderClientUserId);

        /// <summary>
        /// Get a room player by player id
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public static RoomPlayer GetRoomPlayer(int playerId) => !InRoom ? null : Room.RoomPlayerLst.Find(x => x.PlayerId == playerId);

        /// <summary>
        /// Sync the ServerTimestamp with the current millis on EUN Server
        /// </summary>
        /// <param name="onResponse"></param>
        public static void SyncTs(Action<SyncTsOperationResponse> onResponse = null)
        {
            peer.SyncTs(onResponse);
        }

        /// <summary>
        /// Get lobby stats (player count and room count)
        /// </summary>
        /// <param name="skip">How many lobby you want skip?</param>
        /// <param name="limit">How many lobby you want limit?</param>
        /// <param name="onResponse"></param>
        public static void GetLobbyStatsLst(int skip = 0, int limit = 10, Action<GetLobbyStatsLstOperationResponse> onResponse = null)
        {
            peer.GetLobbyStatsLst(skip, limit, onResponse);
        }

        /// <summary>
        /// Get current lobby stats, where client is in, and room lst in this lobby
        /// </summary>
        /// <param name="skip">How many room in this lobby you want skip?</param>
        /// <param name="limit">How many room in this lobby you want limit?</param>
        /// <param name="onResponse"></param>
        public static void GetCurrentLobbyStats(int skip = 0, int limit = 10, Action<GetCurrentLobbyStatsOperationResponse> onResponse = null)
        {
            peer.GetCurrentLobbyStats(skip, limit, onResponse);
        }

        /// <summary>
        /// Join the lobby
        /// If join lobby success, the EUNManagerBehaviour.OnEUNJoinLobby() will callback
        /// </summary>
        /// <param name="lobbyId">The lobby id you want join</param>
        /// <param name="subscriberChat">You want join subscriber chat in this lobby</param>
        /// <param name="onResponse"></param>
        public static void JoinLobby(int lobbyId, bool subscriberChat, Action<JoinLobbyOperationResponse> onResponse = null)
        {
            peer.JoinLobby(lobbyId, response => {
                if (response.Success)
                {
                    if (subscriberChat) peer.SubscriberChatLobby(subscriberChat, null);

                    peer.lobbyId = lobbyId;

                    var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                    for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                    {
                        var behaviour = eunManagerBehaviourLst[i];
                        if (behaviour != null) behaviour.OnEUNJoinLobby();
                    }
                }

                onResponse?.Invoke(response);
            });
        }

        /// <summary>
        /// Join the default lobby, the default lobby id is 0
        /// If join lobby success, the EUNManagerBehaviour.OnEUNJoinLobby() will callback
        /// </summary>
        /// <param name="subscriberChat">You want join subscriber chat in this lobby</param>
        /// <param name="onResponse"></param>
        public static void JoinDefaultLobby(bool subscriberChat, Action<JoinLobbyOperationResponse> onResponse = null)
        {
            JoinLobby(0, subscriberChat, onResponse);
        }

        /// <summary>
        /// Leave current lobby if client is inside lobby
        /// If leave lobby success, the EUNManagerBehaviour.OnEUNLeftLobby() will callback, and the current lobby id is -1
        /// </summary>
        /// <param name="onResponse"></param>
        public static void LeaveLobby(Action<LeaveLobbyOperationResponse> onResponse = null)
        {
            peer.LeaveLobby(response => {
                if (response.Success)
                {
                    peer.lobbyId = -1;

                    var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                    for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                    {
                        var behaviour = eunManagerBehaviourLst[i];
                        if (behaviour != null) behaviour.OnEUNLeftLobby();
                    }
                }

                onResponse?.Invoke(response);
            });
        }

        /// <summary>
        /// Chat to all online player, and they has subscriber chat in EUN Server
        /// If this request send success, the EUNManagerBehaviour.OnEUNReceiveChatAll() will callback
        /// </summary>
        /// <param name="message">The message you want send</param>
        public static void ChatAll(string message)
        {
            peer.ChatAll(message);
        }

        /// <summary>
        /// Chat to all player in client lobby, and they has subscriber chat in lobby
        /// If this request send success, the EUNManagerBehaviour.OnEUNReceiveChatLobby() will callback
        /// </summary>
        /// <param name="message">The message you want send</param>
        public static void ChatLobby(string message)
        {
            peer.ChatLobby(message);
        }

        /// <summary>
        /// Chat to all player in client room
        /// If this request send success, the EUNManagerBehaviour.OnEUNReceiveChatRoom() and EUNBehaviour.OnEUNReceiveChatRoom() will callback
        /// </summary>
        /// <param name="message">The message you want send</param>
        public static void ChatRoom(string message)
        {
            peer.ChatRoom(message);
        }

        /// <summary>
        /// Subscriber for receive chat all, if isSubscribe true, you will receive EUNManagerBehaviour.OnEUNReceiveChatAll() callback whenever someone send request ChatAll()
        /// </summary>
        /// <param name="isSubscribe"></param>
        /// <param name="onResponse"></param>
        public static void SubscriberChatAll(bool isSubscribe, Action<SubscriberChatAllOperationResponse> onResponse = null)
        {
            peer.SubscriberChatAll(isSubscribe, onResponse);
        }

        /// <summary>
        /// Subscriber for receive chat lobby, if isSubscribe true, you will receive EUNManagerBehaviour.OnEUNReceiveChatLobby() callback whenever someone send request ChatLobby()
        /// </summary>
        /// <param name="isSubscribe"></param>
        /// <param name="onResponse"></param>
        public static void SubscriberChatLobby(bool isSubscribe, Action<SubscriberChatLobbyOperationResponse> onResponse = null)
        {
            peer.SubscriberChatLobby(isSubscribe, onResponse);
        }

        /// <summary>
        /// Create a new empty room in this lobby
        /// You only call this request if client is does not inroom, and inside lobby
        /// </summary>
        /// <param name="roomOption">The room infomation</param>
        /// <param name="onResponse"></param>
        public static void CreateRoom(RoomOption roomOption, Action<CreateRoomOperationResponse> onResponse = null)
        {
            peer.CreateRoom(roomOption, onResponse);
        }

        /// <summary>
        /// Join a random room if target expected count and expected properties is match with valid room inside client lobby
        /// If request success, the EUNManagerBehaviour.OnEUNJoinRoom() callback
        /// </summary>
        /// <param name="targetExpectedCount"></param>
        /// <param name="expectedProperties"></param>
        /// <param name="onResponse"></param>
        public static void JoinRandomRoom(int targetExpectedCount, EUNHashtable expectedProperties, Action<JoinRandomRoomOperationResponse> onResponse = null)
        {
            peer.JoinRandomRoom(targetExpectedCount, expectedProperties, onResponse);
        }

        /// <summary>
        /// Join a random room if target expected count and expected properties is match with valid room inside client lobby
        /// If server can not found a valid room for client, it will create a new room
        /// If request success, the EUNManagerBehaviour.OnEUNJoinRoom() callback
        /// </summary>
        /// <param name="targetExpectedCount">How many expected count in expected properties match to join room?</param>
        /// <param name="expectedProperties">The properties can be found with CustomRoomPropertiesForLobby in room</param>
        /// <param name="roomOption">The room infomation to create new room</param>
        /// <param name="onResponse"></param>
        public static void JoinOrCreateRoom(int targetExpectedCount, EUNHashtable expectedProperties, RoomOption roomOption, Action<JoinOrCreateRoomOperationResponse> onResponse = null)
        {
            peer.JoinOrCreateRoom(targetExpectedCount, expectedProperties, roomOption, onResponse);
        }

        /// <summary>
        /// Join a room in this current lobby if this room valid
        /// If request success, the EUNManagerBehaviour.OnEUNJoinRoom() callback
        /// </summary>
        /// <param name="roomId">The room id of this room</param>
        /// <param name="password">The password of this room</param>
        /// <param name="onResponse"></param>
        public static void JoinRoom(int roomId, string password = null, Action<JoinRoomOperationResponse> onResponse = null)
        {
            peer.JoinRoom(roomId, password, onResponse);
        }

        /// <summary>
        /// Leave current room
        /// If request success, the EUNManagerBehaviour.OnEUNLeftRoom() callback
        /// </summary>
        /// <param name="roomId">The room id of this room</param>
        /// <param name="password">The password of this room</param>
        /// <param name="onResponse"></param>
        public static void LeaveRoom(Action<LeaveRoomOperationResponse> onResponse = null)
        {
            peer.LeaveRoom(onResponse);
        }

        /// <summary>
        /// Change new client in room
        /// Only leader client room can change new client
        /// If request success, the EUNManagerBehaviour.OnEUNLeaderClientChange(), EUNBehaviour.OnEUNLeaderClientChange() in other client in this room will callback
        /// </summary>
        /// <param name="leaderClientPlayerId">The player id in this client room</param>
        /// <param name="onResponse"></param>
        public static void ChangeLeaderClient(int leaderClientPlayerId, Action<ChangeLeaderClientOperationResponse> onResponse = null)
        {
            peer.ChangeLeaderClient(leaderClientPlayerId, onResponse);
        }

        /// <summary>
        /// Change the player custom properties of client with player id in this room
        /// If request success, the EUNManagerBehaviour.OnEUNCustomPlayerPropertiesChange(), EUNBehaviour.OnEUNCustomPlayerPropertiesChange() in other client in this room callback
        /// </summary>
        /// <param name="playerId">Player id in this room need change custom player properties</param>
        /// <param name="customPlayerProperties">The custom player properties change</param>
        /// <param name="onResponse"></param>
        public static void ChangePlayerCustomProperties(int playerId, EUNHashtable customPlayerProperties, Action<ChangePlayerCustomPropertiesOperationResponse> onResponse = null)
        {
            peer.ChangePlayerCustomProperties(playerId, customPlayerProperties, onResponse);
        }

        /// <summary>
        /// Change the room info of in this client room
        /// If request success, the EUNBehaviour.OnEUNRoomInfoChange(), EUNBehaviour.OnEUNCustomRoomPropertiesChange(), EUNManagerBehaviour.OnEUNRoomInfoChange(), EUNManagerBehaviour.OnEUNCustomRoomPropertiesChange() callback
        /// </summary>
        /// <param name="eunHashtable">hashtable need change</param>
        /// <param name="onResponse"></param>
        public static void ChangeRoomInfo(EUNHashtable eunHashtable, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            peer.ChangeRoomInfo(eunHashtable, onResponse);
        }

        /// <summary>
        /// Create a new game object room.
        /// If request success, the EUNManagerBehaviour.OnEUNViewNeedCreate() callback
        /// </summary>
        /// <param name="prefabPath">The prefab path of game object</param>
        /// <param name="initializeData">The initialize data, it should be EUNArray or object[]</param>
        /// <param name="synchronizationData">The synchronization data, it should be EUNArray or object[]</param>
        /// <param name="customGameObjectProperties">The custom game object properties</param>
        /// <param name="onResponse"></param>
        public static void CreateGameObjectRoom(string prefabPath, object initializeData, object synchronizationData, EUNHashtable customGameObjectProperties, Action<CreateGameObjectRoomOperationResponse> onResponse = null)
        {
            peer.CreateGameObjectRoom(prefabPath, initializeData, synchronizationData, customGameObjectProperties, onResponse);
        }

        /// <summary>
        /// Change game object room custom properties
        /// If request success, the EUNManagerBehaviour.OnEUNCustomGameObjectPropertiesChange() and EUNBehaviour.OnEUNCustomGameObjectPropertiesChange() callback
        /// </summary>
        /// <param name="objectId">The object id of game object room</param>
        /// <param name="customGameObjectProperties">The custom game object properties</param>
        /// <param name="onResponse"></param>
        public static void ChangeGameObjectCustomProperties(int objectId, EUNHashtable customGameObjectProperties, Action<ChangeGameObjectRoomOperationResponse> onResponse = null)
        {
            peer.ChangeGameObjectCustomProperties(objectId, customGameObjectProperties, onResponse);
        }

        /// <summary>
        /// Destroy game object room
        /// If request success, the EUNManagerBehaviour.OnEUNDestroyGameObjectRoom() and EUNBehaviour.OnEUNDestroyGameObjectRoom() callback
        /// </summary>
        /// <param name="objectId">The object id of game object room</param>
        /// <param name="onResponse"></param>
        public static void DestroyGameObjectRoom(int objectId, Action<DestroyGameObjectRoomRoomOperationResponse> onResponse = null)
        {
            peer.DestroyGameObjectRoom(objectId, onResponse);
        }

        /// <summary>
        /// Transfer new onwer game object room
        /// If request success, the EUNManagerBehaviour.OnEUNTransferOwnerGameObject() and EUNBehaviour.OnEUNTransferOwnerGameObject() callback
        /// </summary>
        /// <param name="objectId">The object id of game object room</param>
        /// <param name="ownerId">The new owner id for this game object room</param>
        /// <param name="onResponse"></param>
        public static void TransferOwnerGameObjectRoom(int objectId, int ownerId, Action<TransferOwnerGameObjectRoomOperationResponse> onResponse = null)
        {
            peer.TransferOwnerGameObjectRoom(objectId, ownerId, onResponse);
        }

        /// <summary>
        /// Send RPC by game object room to special targets 
        /// If request success, the internal EUNBehaviour.EUNRPC() will call
        /// </summary>
        /// <param name="targets">The targets you want send</param>
        /// <param name="objectId">The object id of game object room</param>
        /// <param name="eunRPCCommand">The command id</param>
        /// <param name="rpcData">The rpc data you want send, it should be EUNArray or object[]</param>
        public static void RpcGameObjectRoom(EUNTargets targets, int objectId, int eunRPCCommand, object rpcData)
        {
            var rpcDataArray = new EUNArray.Builder().AddAll(rpcData as object[]).Build();
            
            if (targets == EUNTargets.OnlyMe)
            {
                if (peer.eunViewDic.ContainsKey(objectId))
                {
                    var view = peer.eunViewDic[objectId];
                    if (view)
                    {
                        foreach (var behaviour in view.eunBehaviourLst)
                        {
                            if (behaviour != null) behaviour.EUNRPC(eunRPCCommand, rpcDataArray);
                        }
                    }

                    return;
                }

                for (var i = 0; i < peer.eunManagerBehaviourLst.Count; i++)
                {
                    if (peer.eunManagerBehaviourLst[i] is EUNManagerBehaviour eunManagerBehaviour)
                    {
                        if (eunManagerBehaviour)
                        {
                            var view = eunManagerBehaviour.eunView;
                            if (view.RoomGameObject.ObjectId == objectId)
                            {
                                eunManagerBehaviour.EUNRPC(eunRPCCommand, rpcDataArray);
                                return;
                            }
                        }
                    }
                }
            }
            else if (targets == EUNTargets.LeaderClient)
            {
                if (EUNNetwork.IsLeaderClient)
                {
                    if (peer.eunViewDic.ContainsKey(objectId))
                    {
                        var view = peer.eunViewDic[objectId];
                        if (view)
                        {
                            foreach (var behaviour in view.eunBehaviourLst)
                            {
                                if (behaviour != null) behaviour.EUNRPC(eunRPCCommand, rpcDataArray);
                            }
                        }

                        return;
                    }

                    for (var i = 0; i < peer.eunManagerBehaviourLst.Count; i++)
                    {
                        if (peer.eunManagerBehaviourLst[i] is EUNManagerBehaviour eunManagerBehaviour)
                        {
                            if (eunManagerBehaviour)
                            {
                                var view = eunManagerBehaviour.eunView;
                                if (view.RoomGameObject.ObjectId == objectId)
                                {
                                    eunManagerBehaviour.EUNRPC(eunRPCCommand, rpcDataArray);
                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (EUNNetwork.RoomPlayers.Length > 1)
                    {
                        peer.RpcGameObjectRoom(targets, objectId, eunRPCCommand, rpcData);
                    }
                }
            }
            else if (targets == EUNTargets.AllViaServer)
            {
                if (EUNNetwork.RoomPlayers.Length > 1)
                {
                    peer.RpcGameObjectRoom(targets, objectId, eunRPCCommand, rpcData);
                }
                else
                {
                    if (peer.eunViewDic.ContainsKey(objectId))
                    {
                        var view = peer.eunViewDic[objectId];
                        if (view)
                        {
                            foreach (var behaviour in view.eunBehaviourLst)
                            {
                                if (behaviour != null) behaviour.EUNRPC(eunRPCCommand, rpcDataArray);
                            }
                        }

                        return;
                    }

                    for (var i = 0; i < peer.eunManagerBehaviourLst.Count; i++)
                    {
                        if (peer.eunManagerBehaviourLst[i] is EUNManagerBehaviour eunManagerBehaviour)
                        {
                            if (eunManagerBehaviour)
                            {
                                var view = eunManagerBehaviour.eunView;
                                if (view.RoomGameObject.ObjectId == objectId)
                                {
                                    eunManagerBehaviour.EUNRPC(eunRPCCommand, rpcDataArray);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            else if (targets == EUNTargets.All)
            {
                if (EUNNetwork.RoomPlayers.Length > 1)
                {
                    peer.RpcGameObjectRoom(targets, objectId, eunRPCCommand, rpcData);
                }

                if (peer.eunViewDic.ContainsKey(objectId))
                {
                    var view = peer.eunViewDic[objectId];
                    if (view)
                    {
                        foreach (var behaviour in view.eunBehaviourLst)
                        {
                            if (behaviour != null) behaviour.EUNRPC(eunRPCCommand, rpcDataArray);
                        }
                    }

                    return;
                }

                for (var i = 0; i < peer.eunManagerBehaviourLst.Count; i++)
                {
                    if (peer.eunManagerBehaviourLst[i] is EUNManagerBehaviour eunManagerBehaviour)
                    {
                        if (eunManagerBehaviour)
                        {
                            var view = eunManagerBehaviour.eunView;
                            if (view.RoomGameObject.ObjectId == objectId)
                            {
                                eunManagerBehaviour.EUNRPC(eunRPCCommand, rpcDataArray);
                                return;
                            }
                        }
                    }
                }
            }
            else if (targets == EUNTargets.Others)
            {
                if (EUNNetwork.RoomPlayers.Length > 1)
                {
                    peer.RpcGameObjectRoom(targets, objectId, eunRPCCommand, rpcData);
                }
            }
        }

        /// <summary>
        /// Send RPC by game object room to some player id in this room
        /// </summary>
        /// <param name="targetPlayerIds">The target player ids you want send</param>
        /// <param name="objectId">The object id of game object room</param>
        /// <param name="eunRPCCommand">The command id</param>
        /// <param name="rpcData">The rpc data you want send, it should be EUNArray or object[]</param>
        public static void RpcGameObjectRoomTo(List<int> targetPlayerIds, int objectId, int eunRPCCommand, object rpcData)
        {
            peer.RpcGameObjectRoomTo(targetPlayerIds, objectId, eunRPCCommand, rpcData);
        }
    }
}
