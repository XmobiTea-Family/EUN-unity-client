namespace XmobiTea.EUN
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Entity.Response;

    using System;
    using System.Collections.Generic;
    using XmobiTea.EUN.Constant;
    using System.Threading.Tasks;

    public static partial class EUNNetwork
    {
        /// <summary>
        /// The client has connected to EUN server
        /// </summary>
        public static bool isConnected => peer.isConnected;

        /// <summary>
        /// The room that client is inside, it null if the client is does not inroom
        /// You can join the room by request success: CreateRoom(), JoinOrCreateRoom() or JoinRandomRoom()
        /// </summary>
        public static Room room => peer.room;

        /// <summary>
        /// The client has inroom
        /// </summary>
        public static bool inRoom => room != null;

        /// <summary>
        /// The lobby id, where the client is in
        /// If the lobby id is -1, it mean the client is not in any lobby
        /// You can join the lobby by request success: JoinLobby(), JoinDefaultLobby()
        /// </summary>
        public static int lobbyId => peer.lobbyId;

        /// <summary>
        /// The sync current millis in the current EUN server
        /// The ServerTimestamp will sync first after NetworkingPeer.OnAppAccessHandler
        /// You can sync ServerTimestamp by request SyncTs()
        /// </summary>
        public static long serverTimestamp => peer.tsServerTime;

        /// <summary>
        /// The client 's RoomPlayer when client in room, it will null if the client is does not inroom
        /// </summary>
        public static RoomPlayer localPlayer => getRoomPlayer(playerId);

        /// <summary>
        /// The others player in this client room
        /// </summary>
        public static List<RoomPlayer> roomPlayerLst => !inRoom ? new List<RoomPlayer>() : room.getRoomPlayerLst();

        /// <summary>
        /// Get the RoomGameObject by objectId
        /// </summary>
        /// <param name="objectId">Id of object</param>
        /// <returns></returns>
        public static RoomGameObject getRoomGameObject(int objectId) => !inRoom ? null : !room.gameObjectDict.ContainsKey(objectId) ? null : room.gameObjectDict[objectId];

        /// <summary>
        /// You can call this is MasterClient (a client with highest permission inroom)
        /// This is the LeaderClient in this client room
        /// It may null if at this time call, the room has valid leader client
        /// </summary>
        public static RoomPlayer leaderClientPlayer => !inRoom ? null : room.roomPlayerLst.Find(x => x.userId.Equals(room.leaderClientUserId));

        /// <summary>
        /// It is unique id of client in this room
        /// It only valid if client inroom
        /// When client join or create room success, the EUN Server will auto provide a id in room for client.
        /// </summary>
        public static int playerId => peer.playerId;

        /// <summary>
        /// It true if client is Leader client
        /// </summary>
        public static bool isLeaderClient => inRoom && userId.Equals(room.leaderClientUserId);

        /// <summary>
        /// Get a room player by player id
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public static RoomPlayer getRoomPlayer(int playerId) => !inRoom ? null : room.roomPlayerLst.Find(x => x.playerId == playerId);

        /// <summary>
        /// Sync the ServerTimestamp with the current millis on EUN Server
        /// </summary>
        /// <param name="onResponse"></param>
        public static void syncTs(Action<SyncTsOperationResponse> onResponse = null)
        {
            peer.syncTs(onResponse);
        }

        public static async Task<SyncTsOperationResponse> syncTsAsync()
        {
            SyncTsOperationResponse waitingResult = null;

            syncTs(response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Get lobby stats (player count and room count)
        /// </summary>
        /// <param name="skip">How many lobby you want skip?</param>
        /// <param name="limit">How many lobby you want limit?</param>
        /// <param name="onResponse"></param>
        public static void getLobbyStatsLst(int skip = 0, int limit = 10, Action<GetLobbyStatsLstOperationResponse> onResponse = null)
        {
            peer.getLobbyStatsLst(skip, limit, onResponse);
        }

        public static async Task<GetLobbyStatsLstOperationResponse> getLobbyStatsLstAsync(int skip = 0, int limit = 10)
        {
            GetLobbyStatsLstOperationResponse waitingResult = null;

            getLobbyStatsLst(skip, limit, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Get current lobby stats, where client is in, and room lst in this lobby
        /// </summary>
        /// <param name="skip">How many room in this lobby you want skip?</param>
        /// <param name="limit">How many room in this lobby you want limit?</param>
        /// <param name="onResponse"></param>
        public static void getCurrentLobbyStats(int skip = 0, int limit = 10, Action<GetCurrentLobbyStatsOperationResponse> onResponse = null)
        {
            peer.getCurrentLobbyStats(skip, limit, onResponse);
        }

        public static async Task<GetCurrentLobbyStatsOperationResponse> getCurrentLobbyStatsAsync(int skip = 0, int limit = 10)
        {
            GetCurrentLobbyStatsOperationResponse waitingResult = null;

            getCurrentLobbyStats(skip, limit, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Join the lobby
        /// If join lobby success, the EUNManagerBehaviour.OnEUNJoinLobby() will callback
        /// </summary>
        /// <param name="lobbyId">The lobby id you want join</param>
        /// <param name="subscriberChat">You want join subscriber chat in this lobby</param>
        /// <param name="onResponse"></param>
        public static void joinLobby(int lobbyId, bool subscriberChat, Action<JoinLobbyOperationResponse> onResponse = null)
        {
            peer.joinLobby(lobbyId, response => {
                if (response.success)
                {
                    if (subscriberChat) peer.subscriberChatLobby(subscriberChat, null);

                    peer.lobbyId = lobbyId;

                    var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                    for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                    {
                        var behaviour = eunManagerBehaviourLst[i];
                        if (behaviour != null) behaviour.onEUNJoinLobby();
                    }
                }

                onResponse?.Invoke(response);
            });
        }

        public static async Task<JoinLobbyOperationResponse> joinLobbyAsync(int lobbyId, bool subscriberChat)
        {
            JoinLobbyOperationResponse waitingResult = null;

            joinLobby(lobbyId, subscriberChat, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Join the default lobby, the default lobby id is 0
        /// If join lobby success, the EUNManagerBehaviour.OnEUNJoinLobby() will callback
        /// </summary>
        /// <param name="subscriberChat">You want join subscriber chat in this lobby</param>
        /// <param name="onResponse"></param>
        public static void joinDefaultLobby(bool subscriberChat, Action<JoinLobbyOperationResponse> onResponse = null)
        {
            joinLobby(0, subscriberChat, onResponse);
        }

        public static async Task<JoinLobbyOperationResponse> joinDefaultLobbyAsync(bool subscriberChat)
        {
            JoinLobbyOperationResponse waitingResult = null;

            joinDefaultLobby(subscriberChat, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Leave current lobby if client is inside lobby
        /// If leave lobby success, the EUNManagerBehaviour.OnEUNLeftLobby() will callback, and the current lobby id is -1
        /// </summary>
        /// <param name="onResponse"></param>
        public static void leaveLobby(Action<LeaveLobbyOperationResponse> onResponse = null)
        {
            peer.leaveLobby(response => {
                if (response.success)
                {
                    peer.lobbyId = -1;

                    var eunManagerBehaviourLst = peer.eunManagerBehaviourLst;
                    for (var i = 0; i < eunManagerBehaviourLst.Count; i++)
                    {
                        var behaviour = eunManagerBehaviourLst[i];
                        if (behaviour != null) behaviour.onEUNLeftLobby();
                    }
                }

                onResponse?.Invoke(response);
            });
        }

        public static async Task<LeaveLobbyOperationResponse> leaveLobbyAsync()
        {
            LeaveLobbyOperationResponse waitingResult = null;

            leaveLobby(response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Chat to all online player, and they has subscriber chat in EUN Server
        /// If this request send success, the EUNManagerBehaviour.OnEUNReceiveChatAll() will callback
        /// </summary>
        /// <param name="message">The message you want send</param>
        public static void chatAll(string message, Action<ChatAllOperationResponse> onResponse = null)
        {
            peer.chatAll(message, onResponse);
        }

        public static async Task<ChatAllOperationResponse> chatAllAsync(string message)
        {
            ChatAllOperationResponse waitingResult = null;

            chatAll(message, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Chat to all player in client lobby, and they has subscriber chat in lobby
        /// If this request send success, the EUNManagerBehaviour.OnEUNReceiveChatLobby() will callback
        /// </summary>
        /// <param name="message">The message you want send</param>
        public static void chatLobby(string message, Action<ChatLobbyOperationResponse> onResponse = null)
        {
            peer.chatLobby(message, onResponse);
        }

        public static async Task<ChatLobbyOperationResponse> chatLobbyAsync(string message)
        {
            ChatLobbyOperationResponse waitingResult = null;

            chatLobby(message, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Chat to all player in client room
        /// If this request send success, the EUNManagerBehaviour.OnEUNReceiveChatRoom() and EUNBehaviour.OnEUNReceiveChatRoom() will callback
        /// </summary>
        /// <param name="message">The message you want send</param>
        public static void chatRoom(string message, Action<ChatRoomOperationResponse> onResponse = null)
        {
            peer.chatRoom(message, onResponse);
        }

        public static async Task<ChatRoomOperationResponse> chatRoomAsync(string message)
        {
            ChatRoomOperationResponse waitingResult = null;

            chatRoom(message, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Subscriber for receive chat all, if isSubscribe true, you will receive EUNManagerBehaviour.OnEUNReceiveChatAll() callback whenever someone send request ChatAll()
        /// </summary>
        /// <param name="isSubscribe"></param>
        /// <param name="onResponse"></param>
        public static void subscriberChatAll(bool isSubscribe, Action<SubscriberChatAllOperationResponse> onResponse = null)
        {
            peer.subscriberChatAll(isSubscribe, onResponse);
        }

        public static async Task<SubscriberChatAllOperationResponse> subscriberChatAllAsync(bool isSubscribe)
        {
            SubscriberChatAllOperationResponse waitingResult = null;

            subscriberChatAll(isSubscribe, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Subscriber for receive chat lobby, if isSubscribe true, you will receive EUNManagerBehaviour.OnEUNReceiveChatLobby() callback whenever someone send request ChatLobby()
        /// </summary>
        /// <param name="isSubscribe"></param>
        /// <param name="onResponse"></param>
        public static void subscriberChatLobby(bool isSubscribe, Action<SubscriberChatLobbyOperationResponse> onResponse = null)
        {
            peer.subscriberChatLobby(isSubscribe, onResponse);
        }

        public static async Task<SubscriberChatLobbyOperationResponse> subscriberChatLobbyAsync(bool isSubscribe)
        {
            SubscriberChatLobbyOperationResponse waitingResult = null;

            subscriberChatLobby(isSubscribe, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Create a new empty room in this lobby
        /// You only call this request if client is does not inroom, and inside lobby
        /// </summary>
        /// <param name="roomOption">The room infomation</param>
        /// <param name="onResponse"></param>
        public static void createRoom(RoomOption roomOption, Action<CreateRoomOperationResponse> onResponse = null)
        {
            peer.createRoom(roomOption, onResponse);
        }

        public static async Task<CreateRoomOperationResponse> createRoomAsync(RoomOption roomOption)
        {
            CreateRoomOperationResponse waitingResult = null;

            createRoom(roomOption, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Join a random room if target expected count and expected properties is match with valid room inside client lobby
        /// If request success, the EUNManagerBehaviour.OnEUNJoinRoom() callback
        /// </summary>
        /// <param name="targetExpectedCount"></param>
        /// <param name="expectedProperties"></param>
        /// <param name="onResponse"></param>
        public static void joinRandomRoom(int targetExpectedCount, EUNHashtable expectedProperties, Action<JoinRandomRoomOperationResponse> onResponse = null)
        {
            peer.joinRandomRoom(targetExpectedCount, expectedProperties, onResponse);
        }

        public static async Task<JoinRandomRoomOperationResponse> joinRandomRoomAsync(int targetExpectedCount, EUNHashtable expectedProperties)
        {
            JoinRandomRoomOperationResponse waitingResult = null;

            joinRandomRoom(targetExpectedCount, expectedProperties, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
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
        public static void joinOrCreateRoom(int targetExpectedCount, EUNHashtable expectedProperties, RoomOption roomOption, Action<JoinOrCreateRoomOperationResponse> onResponse = null)
        {
            peer.joinOrCreateRoom(targetExpectedCount, expectedProperties, roomOption, onResponse);
        }

        public static async Task<JoinOrCreateRoomOperationResponse> joinOrCreateRoomAsync(int targetExpectedCount, EUNHashtable expectedProperties, RoomOption roomOption)
        {
            JoinOrCreateRoomOperationResponse waitingResult = null;

            joinOrCreateRoom(targetExpectedCount, expectedProperties, roomOption, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Join a room in this current lobby if this room valid
        /// If request success, the EUNManagerBehaviour.OnEUNJoinRoom() callback
        /// </summary>
        /// <param name="roomId">The room id of this room</param>
        /// <param name="password">The password of this room</param>
        /// <param name="onResponse"></param>
        public static void joinRoom(int roomId, string password = null, Action<JoinRoomOperationResponse> onResponse = null)
        {
            peer.joinRoom(roomId, password, onResponse);
        }

        public static async Task<JoinRoomOperationResponse> joinRoomAsync(int roomId, string password = null)
        {
            JoinRoomOperationResponse waitingResult = null;

            joinRoom(roomId, password, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Leave current room
        /// If request success, the EUNManagerBehaviour.OnEUNLeftRoom() callback
        /// </summary>
        /// <param name="roomId">The room id of this room</param>
        /// <param name="password">The password of this room</param>
        /// <param name="onResponse"></param>
        public static void leaveRoom(Action<LeaveRoomOperationResponse> onResponse = null)
        {
            peer.leaveRoom(onResponse);
        }

        public static async Task<LeaveRoomOperationResponse> leaveRoomAsync()
        {
            LeaveRoomOperationResponse waitingResult = null;

            leaveRoom(response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Change new client in room
        /// Only leader client room can change new client
        /// If request success, the EUNManagerBehaviour.OnEUNLeaderClientChange(), EUNBehaviour.OnEUNLeaderClientChange() in other client in this room will callback
        /// </summary>
        /// <param name="leaderClientPlayerId">The player id in this client room</param>
        /// <param name="onResponse"></param>
        public static void changeLeaderClient(int leaderClientPlayerId, Action<ChangeLeaderClientOperationResponse> onResponse = null)
        {
            peer.changeLeaderClient(leaderClientPlayerId, onResponse);
        }

        public static async Task<ChangeLeaderClientOperationResponse> changeLeaderClientAsync(int leaderClientPlayerId)
        {
            ChangeLeaderClientOperationResponse waitingResult = null;

            changeLeaderClient(leaderClientPlayerId, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Change the player custom properties of client with player id in this room
        /// If request success, the EUNManagerBehaviour.OnEUNCustomPlayerPropertiesChange(), EUNBehaviour.OnEUNCustomPlayerPropertiesChange() in other client in this room callback
        /// </summary>
        /// <param name="playerId">Player id in this room need change custom player properties</param>
        /// <param name="customPlayerProperties">The custom player properties change</param>
        /// <param name="onResponse"></param>
        public static void changePlayerCustomProperties(int playerId, EUNHashtable customPlayerProperties, Action<ChangePlayerCustomPropertiesOperationResponse> onResponse = null)
        {
            peer.changePlayerCustomProperties(playerId, customPlayerProperties, onResponse);
        }

        public static async Task<ChangePlayerCustomPropertiesOperationResponse> changePlayerCustomPropertiesAsync(int playerId, EUNHashtable customPlayerProperties)
        {
            ChangePlayerCustomPropertiesOperationResponse waitingResult = null;

            changePlayerCustomProperties(playerId, customPlayerProperties, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Change the room info of in this client room
        /// If request success, the EUNBehaviour.OnEUNRoomInfoChange(), EUNBehaviour.OnEUNCustomRoomPropertiesChange(), EUNManagerBehaviour.OnEUNRoomInfoChange(), EUNManagerBehaviour.OnEUNCustomRoomPropertiesChange() callback
        /// </summary>
        /// <param name="eunHashtable">hashtable need change</param>
        /// <param name="onResponse"></param>
        public static void changeRoomInfo(EUNHashtable eunHashtable, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            peer.changeRoomInfo(eunHashtable, onResponse);
        }

        public static async Task<ChangeRoomInfoOperationResponse> changeRoomInfoAsync(EUNHashtable eunHashtable)
        {
            ChangeRoomInfoOperationResponse waitingResult = null;

            changeRoomInfo(eunHashtable, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
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
        public static void createGameObjectRoom(string prefabPath, object initializeData, object synchronizationData, EUNHashtable customGameObjectProperties, Action<CreateGameObjectRoomOperationResponse> onResponse = null)
        {
            peer.createGameObjectRoom(prefabPath, initializeData, synchronizationData, customGameObjectProperties, onResponse);
        }

        public static async Task<CreateGameObjectRoomOperationResponse> createGameObjectRoomAsync(string prefabPath, object initializeData, object synchronizationData, EUNHashtable customGameObjectProperties)
        {
            CreateGameObjectRoomOperationResponse waitingResult = null;

            createGameObjectRoom(prefabPath, initializeData, synchronizationData, customGameObjectProperties, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Change game object room custom properties
        /// If request success, the EUNManagerBehaviour.OnEUNCustomGameObjectPropertiesChange() and EUNBehaviour.OnEUNCustomGameObjectPropertiesChange() callback
        /// </summary>
        /// <param name="objectId">The object id of game object room</param>
        /// <param name="customGameObjectProperties">The custom game object properties</param>
        /// <param name="onResponse"></param>
        public static void changeGameObjectCustomProperties(int objectId, EUNHashtable customGameObjectProperties, Action<ChangeGameObjectRoomOperationResponse> onResponse = null)
        {
            peer.changeGameObjectCustomProperties(objectId, customGameObjectProperties, onResponse);
        }

        public static async Task<ChangeGameObjectRoomOperationResponse> changeGameObjectCustomPropertiesAsync(int objectId, EUNHashtable customGameObjectProperties)
        {
            ChangeGameObjectRoomOperationResponse waitingResult = null;

            changeGameObjectCustomProperties(objectId, customGameObjectProperties, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Destroy game object room
        /// If request success, the EUNManagerBehaviour.OnEUNDestroyGameObjectRoom() and EUNBehaviour.OnEUNDestroyGameObjectRoom() callback
        /// </summary>
        /// <param name="objectId">The object id of game object room</param>
        /// <param name="onResponse"></param>
        public static void destroyGameObjectRoom(int objectId, Action<DestroyGameObjectRoomRoomOperationResponse> onResponse = null)
        {
            peer.destroyGameObjectRoom(objectId, onResponse);
        }

        public static async Task<DestroyGameObjectRoomRoomOperationResponse> destroyGameObjectRoomAsync(int objectId)
        {
            DestroyGameObjectRoomRoomOperationResponse waitingResult = null;

            destroyGameObjectRoom(objectId, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Transfer new onwer game object room
        /// If request success, the EUNManagerBehaviour.OnEUNTransferOwnerGameObject() and EUNBehaviour.OnEUNTransferOwnerGameObject() callback
        /// </summary>
        /// <param name="objectId">The object id of game object room</param>
        /// <param name="ownerId">The new owner id for this game object room</param>
        /// <param name="onResponse"></param>
        public static void transferOwnerGameObjectRoom(int objectId, int ownerId, Action<TransferOwnerGameObjectRoomOperationResponse> onResponse = null)
        {
            peer.transferOwnerGameObjectRoom(objectId, ownerId, onResponse);
        }

        public static async Task<TransferOwnerGameObjectRoomOperationResponse> transferOwnerGameObjectRoomAsync(int objectId, int ownerId)
        {
            TransferOwnerGameObjectRoomOperationResponse waitingResult = null;

            transferOwnerGameObjectRoom(objectId, ownerId, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Send RPC by game object room to special targets 
        /// If request success, the internal EUNBehaviour.EUNRPC() will call
        /// </summary>
        /// <param name="targets">The targets you want send</param>
        /// <param name="objectId">The object id of game object room</param>
        /// <param name="eunRPCCommand">The command id</param>
        /// <param name="rpcData">The rpc data you want send, it should be EUNArray or object[]</param>
        public static void rpcGameObjectRoom(EUNTargets targets, int objectId, int eunRPCCommand, object rpcData, Action<RpcGameObjectRoomOperationResponse> onResponse = null)
        {
            var rpcDataArray = new EUNArray.Builder().addAll(rpcData as object[]).build();
            
            if (targets == EUNTargets.OnlyMe)
            {
                if (peer.eunViewDict.ContainsKey(objectId))
                {
                    var view = peer.eunViewDict[objectId];
                    if (view)
                    {
                        foreach (var behaviour in view.eunBehaviourLst)
                        {
                            if (behaviour != null) behaviour.eunRpc(eunRPCCommand, rpcDataArray);
                        }
                    }
                }
            }
            else if (targets == EUNTargets.LeaderClient)
            {
                if (EUNNetwork.isLeaderClient)
                {
                    if (peer.eunViewDict.ContainsKey(objectId))
                    {
                        var view = peer.eunViewDict[objectId];
                        if (view)
                        {
                            foreach (var behaviour in view.eunBehaviourLst)
                            {
                                if (behaviour != null) behaviour.eunRpc(eunRPCCommand, rpcDataArray);
                            }
                        }
                    }
                }
                else
                {
                    if (EUNNetwork.roomPlayerLst.Count > 1)
                    {
                        peer.rpcGameObjectRoom(targets, objectId, eunRPCCommand, rpcData, onResponse);
                    }
                }
            }
            else if (targets == EUNTargets.AllViaServer)
            {
                if (EUNNetwork.roomPlayerLst.Count > 1)
                {
                    peer.rpcGameObjectRoom(targets, objectId, eunRPCCommand, rpcData, onResponse);
                }
                else
                {
                    if (peer.eunViewDict.ContainsKey(objectId))
                    {
                        var view = peer.eunViewDict[objectId];
                        if (view)
                        {
                            foreach (var behaviour in view.eunBehaviourLst)
                            {
                                if (behaviour != null) behaviour.eunRpc(eunRPCCommand, rpcDataArray);
                            }
                        }
                    }
                }
            }
            else if (targets == EUNTargets.All)
            {
                if (EUNNetwork.roomPlayerLst.Count > 1)
                {
                    peer.rpcGameObjectRoom(targets, objectId, eunRPCCommand, rpcData, onResponse);
                }

                if (peer.eunViewDict.ContainsKey(objectId))
                {
                    var view = peer.eunViewDict[objectId];
                    if (view)
                    {
                        foreach (var behaviour in view.eunBehaviourLst)
                        {
                            if (behaviour != null) behaviour.eunRpc(eunRPCCommand, rpcDataArray);
                        }
                    }
                }
            }
            else if (targets == EUNTargets.Others)
            {
                if (EUNNetwork.roomPlayerLst.Count > 1)
                {
                    peer.rpcGameObjectRoom(targets, objectId, eunRPCCommand, rpcData, onResponse);
                }
            }
        }

        public static async Task<RpcGameObjectRoomOperationResponse> rpcGameObjectRoomAsync(EUNTargets targets, int objectId, int eunRPCCommand, object rpcData)
        {
            RpcGameObjectRoomOperationResponse waitingResult = null;

            rpcGameObjectRoom(targets, objectId, eunRPCCommand, rpcData, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Send RPC by game object room to some player id in this room
        /// </summary>
        /// <param name="targetPlayerIds">The target player ids you want send</param>
        /// <param name="objectId">The object id of game object room</param>
        /// <param name="eunRPCCommand">The command id</param>
        /// <param name="rpcData">The rpc data you want send, it should be EUNArray or object[]</param>
        public static void rpcGameObjectRoomTo(List<int> targetPlayerIds, int objectId, int eunRPCCommand, object rpcData, Action<RpcGameObjectRoomToOperationResponse> onResponse = null)
        {
            peer.rpcGameObjectRoomTo(targetPlayerIds, objectId, eunRPCCommand, rpcData, onResponse);
        }

        public static async Task<RpcGameObjectRoomToOperationResponse> rpcGameObjectRoomToAsync(List<int> targetPlayerIds, int objectId, int eunRPCCommand, object rpcData)
        {
            RpcGameObjectRoomToOperationResponse waitingResult = null;

            rpcGameObjectRoomTo(targetPlayerIds, objectId, eunRPCCommand, rpcData, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

    }

}
