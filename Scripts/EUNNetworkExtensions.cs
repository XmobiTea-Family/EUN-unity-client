namespace XmobiTea.EUN.Extension
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Entity.Response;

    using System;

    public static partial class EUNNetworkExtensions
    {
        #region Extension

        /// <summary>
        /// Join this lobby
        /// If join lobby success, the EUNManagerBehaviour.OnEUNJoinLobby() will callback
        /// </summary>
        /// <param name="lobbyStats">This lobby stats</param>
        /// <param name="subscriberChat">You want join subscriber chat in this lobby</param>
        /// <param name="onResponse"></param>
        public static void JoinLobby(this LobbyStats lobbyStats, bool subscriberChat, Action<JoinLobbyOperationResponse> onResponse = null)
        {
            EUNNetwork.JoinLobby(lobbyStats.lobbyId, subscriberChat, response => {
                onResponse?.Invoke(response);
            });
        }

        /// <summary>
        /// Change new client in room
        /// Only leader client room can change new client
        /// If request success, the EUNManagerBehaviour.OnEUNLeaderClientChange(), EUNBehaviour.OnEUNLeaderClientChange() in other client in this room will callback
        /// </summary>
        /// <param name="newLeaderClient">The new Leader Client</param>
        /// <param name="onResponse"></param>
        public static void ChangeLeaderClient(this RoomPlayer newLeaderClient, Action<ChangeLeaderClientOperationResponse> onResponse = null)
        {
            EUNNetwork.ChangeLeaderClient(newLeaderClient.playerId, onResponse);
        }

        /// <summary>
        /// Change the player custom properties of roomPlayer in this room
        /// If request success, the EUNManagerBehaviour.OnEUNCustomPlayerPropertiesChange(), EUNBehaviour.OnEUNCustomPlayerPropertiesChange() in other client in this room callback
        /// </summary>
        /// <param name="roomPlayer">The room player you want to change</param>
        /// <param name="customPlayerProperties">The custom player properties change</param>
        /// <param name="onResponse"></param>
        public static void ChangeCustomProperties(this RoomPlayer roomPlayer, EUNHashtable customPlayerProperties, Action<ChangePlayerCustomPropertiesOperationResponse> onResponse = null)
        {
            EUNNetwork.ChangePlayerCustomProperties(roomPlayer.playerId, customPlayerProperties, onResponse);
        }

        /// <summary>
        /// Transfer new onwer game object room in eunView, to the roomPlayer in this room
        /// If request success, the EUNManagerBehaviour.OnEUNTransferOwnerGameObject() and EUNBehaviour.OnEUNTransferOwnerGameObject() callback
        /// </summary>
        /// <param name="newOwner">The new owner in current room for this eunView</param>
        /// <param name="eunView">The eunView need change new owner</param>
        /// <param name="onResponse"></param>
        public static void TransferOwnerGameObjectRoom(this RoomPlayer newOwner, EUNView eunView, Action<TransferOwnerGameObjectRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.TransferOwnerGameObjectRoom(eunView.roomGameObject.objectId, newOwner.playerId, onResponse);
        }

        /// <summary>
        /// Transfer new onwer game object room in eunView, to the roomPlayer in this room
        /// If request success, the EUNManagerBehaviour.OnEUNTransferOwnerGameObject() and EUNBehaviour.OnEUNTransferOwnerGameObject() callback
        /// </summary>
        /// <param name="newOwner">The new owner in current room for this eunView</param>
        /// <param name="eunView">The eunView need change new owner</param>
        /// <param name="onResponse"></param>
        public static void TransferOwnerGameObjectRoom(this EUNView eunView, RoomPlayer newOwner, Action<TransferOwnerGameObjectRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.TransferOwnerGameObjectRoom(eunView.roomGameObject.objectId, newOwner.playerId, onResponse);
        }

        /// <summary>
        /// Transfer new onwer game object room in eunView, to the roomPlayer in this room
        /// If request success, the EUNManagerBehaviour.OnEUNTransferOwnerGameObject() and EUNBehaviour.OnEUNTransferOwnerGameObject() callback
        /// </summary>
        /// <param name="newOwnerId">The new owner player id in current room for this eunView</param>
        /// <param name="eunView">The eunView need change new owner</param>
        /// <param name="onResponse"></param>
        public static void TransfeOwnerGameObjectRoom(this EUNView eunView, int newOwnerId, Action<TransferOwnerGameObjectRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.TransferOwnerGameObjectRoom(eunView.roomGameObject.objectId, newOwnerId, onResponse);
        }

        /// <summary>
        /// Send a RPC command by eunView
        /// If request success, the internal EUNBehaviour.EUNRPC() will call
        /// </summary>
        /// <param name="eunView">The eunView need send RPC</param>
        /// <param name="targets">The targets need send to</param>
        /// <param name="command">The command need send</param>
        /// <param name="rpcData">The rpc data attach with command</param>
        public static void RPC(this EUNView eunView, EUNTargets targets, EUNRPCCommand command, params object[] rpcData)
        {
            EUNNetwork.RpcGameObjectRoom(targets, eunView.roomGameObject.objectId, (int)command, rpcData);
        }

        /// <summary>
        /// Send destroy the game object room
        /// If request success, the EUNManagerBehaviour.OnEUNDestroyGameObjectRoom() and EUNBehaviour.OnEUNDestroyGameObjectRoom() callback
        /// </summary>
        /// <param name="roomGameObject">The room game object need destroy</param>
        /// <param name="onResponse"></param>
        public static void DestroyGameObjectRoom(this RoomGameObject roomGameObject, Action<DestroyGameObjectRoomRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.DestroyGameObjectRoom(roomGameObject.objectId, onResponse);
        }

        /// <summary>
        /// Send destroy the game object room in eunView
        /// If request success, the EUNManagerBehaviour.OnEUNDestroyGameObjectRoom() and EUNBehaviour.OnEUNDestroyGameObjectRoom() callback
        /// </summary>
        /// <param name="eunView">The eunView need destroy</param>
        /// <param name="onResponse"></param>
        public static void DestroyGameObjectRoom(this EUNView eunView, Action<DestroyGameObjectRoomRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.DestroyGameObjectRoom(eunView.roomGameObject.objectId, onResponse);
        }

        /// <summary>
        /// Change the custom properties of roomGameObject
        /// If request success, the EUNManagerBehaviour.OnEUNCustomGameObjectPropertiesChange() and EUNBehaviour.OnEUNCustomGameObjectPropertiesChange() callback
        /// </summary>
        /// <param name="roomGameObject">The room game object need change custom properties</param>
        /// <param name="customProperties">The custom properties you want change</param>
        /// <param name="onResponse"></param>
        public static void ChangeGameObjectCustomProperties(this RoomGameObject roomGameObject, EUNHashtable customProperties, Action<ChangeGameObjectRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.ChangeGameObjectCustomProperties(roomGameObject.objectId, customProperties, onResponse);
        }

        /// <summary>
        /// Change the custom properties of roomGameObject
        /// If request success, the EUNManagerBehaviour.OnEUNCustomGameObjectPropertiesChange() and EUNBehaviour.OnEUNCustomGameObjectPropertiesChange() callback
        /// </summary>
        /// <param name="eunView">The room game object in eunView need change custom properties</param>
        /// <param name="customProperties">The custom properties you want change</param>
        /// <param name="onResponse"></param>
        public static void ChangeGameObjectCustomProperties(this EUNView eunView, EUNHashtable customProperties, Action<ChangeGameObjectRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.ChangeGameObjectCustomProperties(eunView.roomGameObject.objectId, customProperties, onResponse);
        }

        /// <summary>
        /// Join this room if you are not inroom
        /// If the password right, and the room valid, you can join room
        /// If request success, the EUNManagerBehaviour.OnEUNJoinRoom() callback
        /// </summary>
        /// <param name="lobbyRoomStats">The lobby room stats you want join room</param>
        /// <param name="password">The password for lobby room</param>
        /// <param name="onResponse"></param>
        public static void JoinRoom(this LobbyRoomStats lobbyRoomStats, string password = null, Action<JoinRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.JoinRoom(lobbyRoomStats.roomId, password, onResponse);
        }

        /// <summary>
        /// Leave this room
        /// If request success, the EUNManagerBehaviour.OnEUNLeftRoom() callback
        /// </summary>
        /// <param name="room">This room</param>
        /// <param name="onResponse"></param>
        public static void LeaveRoom(this Room room, Action<LeaveRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.LeaveRoom(onResponse);
        }

        /// <summary>
        /// Change custom properties for this room if client inroom, any client in this room can change room custom properties
        /// If request success, EUNBehaviour.OnEUNCustomRoomPropertiesChange(), EUNManagerBehaviour.OnEUNCustomRoomPropertiesChange callback
        /// </summary>
        /// <param name="room">This room</param>
        /// <param name="customRoomProperties">The custom room properties need change</param>
        /// <param name="onResponse"></param>
        public static void ChangeCustomProperties(this Room room, EUNHashtable customRoomProperties, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                .Add(ParameterCode.CustomRoomProperties, customRoomProperties)
                .Build();

            EUNNetwork.ChangeRoomInfo(eunHashtable, onResponse);
        }

        /// <summary>
        /// Change the max player in this room, only leader client can send this request
        /// The max player can not less than current player in current room, and can not less than 1
        /// If request success, the EUNBehaviour.OnEUNRoomInfoChange(), EUNManagerBehaviour.OnEUNRoomInfoChange() callback
        /// </summary>
        /// <param name="room">This room</param>
        /// <param name="maxPlayer">The max player need change</param>
        /// <param name="onResponse"></param>
        public static void ChangeMaxPlayer(this Room room, int maxPlayer, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                            .Add(ParameterCode.MaxPlayer, maxPlayer)
                            .Build();

            EUNNetwork.ChangeRoomInfo(eunHashtable, onResponse);
        }

        /// <summary>
        /// Change the custom properties for lobby in this room
        /// Only leader client can send this request
        /// If request success, the EUNBehaviour.OnEUNRoomInfoChange(), EUNManagerBehaviour.OnEUNRoomInfoChange() callback
        /// </summary>
        /// <param name="room">This room</param>
        /// <param name="customRoomPropertiesForLobby">The custom properties for lobby need change</param>
        /// <param name="onResponse"></param>
        public static void ChangeCustomPropertiesForLobby(this Room room, EUNHashtable customRoomPropertiesForLobby, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                            .Add(ParameterCode.CustomRoomPropertiesForLobby, customRoomPropertiesForLobby)
                            .Build();

            EUNNetwork.ChangeRoomInfo(eunHashtable, onResponse);
        }

        /// <summary>
        /// Change the isOpen in this room
        /// If the isOpen == false, no client can join this room
        /// Only leader client can send this request
        /// If request success, the EUNBehaviour.OnEUNRoomInfoChange(), EUNManagerBehaviour.OnEUNRoomInfoChange() callback
        /// </summary>
        /// <param name="room">This room</param>
        /// <param name="isOpen">The open for this room</param>
        /// <param name="onResponse"></param>
        public static void ChangeOpen(this Room room, bool isOpen, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                            .Add(ParameterCode.IsOpen, isOpen)
                            .Build();

            EUNNetwork.ChangeRoomInfo(eunHashtable, onResponse);
        }

        /// <summary>
        /// Change the isVisible in this room
        /// If the isVisible == false, no client can found this room in GetCurrentLobbyStats() or GetLobbyStats()
        /// Only leader client can send this request
        /// If request success, the EUNBehaviour.OnEUNRoomInfoChange(), EUNManagerBehaviour.OnEUNRoomInfoChange() callback
        /// </summary>
        /// <param name="room">This room</param>
        /// <param name="isVisible">The visible for this room</param>
        /// <param name="onResponse"></param>
        public static void ChangeVisible(this Room room, bool isVisible, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                            .Add(ParameterCode.IsVisible, isVisible)
                            .Build();

            EUNNetwork.ChangeRoomInfo(eunHashtable, onResponse);
        }

        /// <summary>
        /// Change the password in this room
        /// Only leader client can send this request
        /// Other clients not join this room need password right to join this room
        /// Remove password room if set password = null
        /// If request success, the EUNBehaviour.OnEUNRoomInfoChange(), EUNManagerBehaviour.OnEUNRoomInfoChange() callback
        /// </summary>
        /// <param name="room">This room</param>
        /// <param name="password">The password for this room</param>
        /// <param name="onResponse"></param>
        public static void ChangePassword(this Room room, string password, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                            .Add(ParameterCode.Password, password)
                            .Build();

            EUNNetwork.ChangeRoomInfo(eunHashtable, onResponse);
        }

        /// <summary>
        /// Change the time to live in this room
        /// Time to live mean time client can rejoin for this room after they disconnect
        /// When nobody in this room after they disconnect, and end time to live, the room will remove in lobby
        /// Only leader client can send this request
        /// If request success, the EUNBehaviour.OnEUNRoomInfoChange(), EUNManagerBehaviour.OnEUNRoomInfoChange() callback
        /// </summary>
        /// <param name="room">This room</param>
        /// <param name="ttl">The time to live for this room</param>
        /// <param name="onResponse"></param>
        public static void ChangeTtl(this Room room, int ttl, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                .Add(ParameterCode.Ttl, ttl)
                .Build();

            EUNNetwork.ChangeRoomInfo(eunHashtable, onResponse);
        }

        /// <summary>
        /// Create the game object in this room
        /// </summary>
        /// <param name="room">This room</param>
        /// <param name="prefabPath">The prefab path of room game object</param>
        /// <param name="initializeData">The initialize data for room game object</param>
        /// <param name="synchronizationData">The synchronization data for room game object</param>
        /// <param name="customGameObjectProperties">The custom room game object properties</param>
        /// <param name="onResponse"></param>
        public static void CreateGameObjectRoom(this Room room, string prefabPath, object initializeData, object synchronizationData, EUNHashtable customGameObjectProperties, Action<CreateGameObjectRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.CreateGameObjectRoom(prefabPath, initializeData, synchronizationData, customGameObjectProperties, onResponse);
        }

#endregion
    }
}
