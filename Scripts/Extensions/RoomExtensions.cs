namespace XmobiTea.EUN.Extensions
{
    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Entity.Response;

    using System;
    using System.Threading.Tasks;
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Common;

    public static class RoomExtensions
    {
        /// <summary>
        /// Leave this room
        /// If request success, the EUNManagerBehaviour.OnEUNLeftRoom() callback
        /// </summary>
        /// <param name="room">This room</param>
        /// <param name="onResponse"></param>
        public static void leaveRoom(this Room room, Action<LeaveRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.leaveRoom(onResponse);
        }

        public static async Task<LeaveRoomOperationResponse> leaveRoomAsync(this Room room)
        {
            LeaveRoomOperationResponse waitingResult = null;

            leaveRoom(room, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Change custom properties for this room if client inroom, any client in this room can change room custom properties
        /// If request success, EUNBehaviour.OnEUNCustomRoomPropertiesChange(), EUNManagerBehaviour.OnEUNCustomRoomPropertiesChange callback
        /// </summary>
        /// <param name="room">This room</param>
        /// <param name="customRoomProperties">The custom room properties need change</param>
        /// <param name="onResponse"></param>
        public static void changeCustomProperties(this Room room, EUNHashtable customRoomProperties, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                .add(ParameterCode.CustomRoomProperties, customRoomProperties)
                .build();

            EUNNetwork.changeRoomInfo(eunHashtable, onResponse);
        }

        public static async Task<ChangeRoomInfoOperationResponse> changeCustomPropertiesAsync(this Room room, EUNHashtable customRoomProperties)
        {
            ChangeRoomInfoOperationResponse waitingResult = null;

            changeCustomProperties(room, customRoomProperties, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Change the max player in this room, only leader client can send this request
        /// The max player can not less than current player in current room, and can not less than 1
        /// If request success, the EUNBehaviour.OnEUNRoomInfoChange(), EUNManagerBehaviour.OnEUNRoomInfoChange() callback
        /// </summary>
        /// <param name="room">This room</param>
        /// <param name="maxPlayer">The max player need change</param>
        /// <param name="onResponse"></param>
        public static void changeMaxPlayer(this Room room, int maxPlayer, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                            .add(ParameterCode.MaxPlayer, maxPlayer)
                            .build();

            EUNNetwork.changeRoomInfo(eunHashtable, onResponse);
        }

        public static async Task<ChangeRoomInfoOperationResponse> changeMaxPlayerAsync(this Room room, int maxPlayer)
        {
            ChangeRoomInfoOperationResponse waitingResult = null;

            changeMaxPlayer(room, maxPlayer, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Change the custom properties for lobby in this room
        /// Only leader client can send this request
        /// If request success, the EUNBehaviour.OnEUNRoomInfoChange(), EUNManagerBehaviour.OnEUNRoomInfoChange() callback
        /// </summary>
        /// <param name="room">This room</param>
        /// <param name="customRoomPropertiesForLobby">The custom properties for lobby need change</param>
        /// <param name="onResponse"></param>
        public static void changeCustomPropertiesForLobby(this Room room, EUNHashtable customRoomPropertiesForLobby, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                            .add(ParameterCode.CustomRoomPropertiesForLobby, customRoomPropertiesForLobby)
                            .build();

            EUNNetwork.changeRoomInfo(eunHashtable, onResponse);
        }

        public static async Task<ChangeRoomInfoOperationResponse> changeCustomPropertiesForLobbyAsync(this Room room, EUNHashtable customRoomPropertiesForLobby)
        {
            ChangeRoomInfoOperationResponse waitingResult = null;

            changeCustomPropertiesForLobby(room, customRoomPropertiesForLobby, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
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
        public static void changeOpen(this Room room, bool isOpen, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                            .add(ParameterCode.IsOpen, isOpen)
                            .build();

            EUNNetwork.changeRoomInfo(eunHashtable, onResponse);
        }

        public static async Task<ChangeRoomInfoOperationResponse> changeOpenAsync(this Room room, bool isOpen)
        {
            ChangeRoomInfoOperationResponse waitingResult = null;

            changeOpen(room, isOpen, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
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
        public static void changeVisible(this Room room, bool isVisible, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                            .add(ParameterCode.IsVisible, isVisible)
                            .build();

            EUNNetwork.changeRoomInfo(eunHashtable, onResponse);
        }

        public static async Task<ChangeRoomInfoOperationResponse> changeVisibleAsync(this Room room, bool isVisible)
        {
            ChangeRoomInfoOperationResponse waitingResult = null;

            changeVisible(room, isVisible, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
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
        public static void changePassword(this Room room, string password, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                            .add(ParameterCode.Password, password)
                            .build();

            EUNNetwork.changeRoomInfo(eunHashtable, onResponse);
        }

        public static async Task<ChangeRoomInfoOperationResponse> changePasswordAsync(this Room room, string password)
        {
            ChangeRoomInfoOperationResponse waitingResult = null;

            changePassword(room, password, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
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
        public static void changeTtl(this Room room, int ttl, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                .add(ParameterCode.Ttl, ttl)
                .build();

            EUNNetwork.changeRoomInfo(eunHashtable, onResponse);
        }

        public static async Task<ChangeRoomInfoOperationResponse> changeTtlAsync(this Room room, int ttl)
        {
            ChangeRoomInfoOperationResponse waitingResult = null;

            changeTtl(room, ttl, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
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
        public static void createGameObjectRoom(this Room room, string prefabPath, object initializeData, object synchronizationData, EUNHashtable customGameObjectProperties, Action<CreateGameObjectRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.createGameObjectRoom(prefabPath, initializeData, synchronizationData, customGameObjectProperties, onResponse);
        }

        public static async Task<CreateGameObjectRoomOperationResponse> createGameObjectRoomAsync(this Room room, string prefabPath, object initializeData, object synchronizationData, EUNHashtable customGameObjectProperties)
        {
            CreateGameObjectRoomOperationResponse waitingResult = null;

            createGameObjectRoom(room, prefabPath, initializeData, synchronizationData, customGameObjectProperties, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }


        /// <summary>
        /// Change new client in room
        /// Only leader client room can change new client
        /// If request success, the EUNManagerBehaviour.OnEUNLeaderClientChange(), EUNBehaviour.OnEUNLeaderClientChange() in other client in this room will callback
        /// </summary>
        /// <param name="newLeaderClient">The new Leader Client</param>
        /// <param name="onResponse"></param>
        public static void changeLeaderClient(this Room room, RoomPlayer newLeaderClient, Action<ChangeLeaderClientOperationResponse> onResponse = null)
        {
            EUNNetwork.changeLeaderClient(newLeaderClient.playerId, onResponse);
        }

        public static async Task<ChangeLeaderClientOperationResponse> changeLeaderClientAsync(this Room room, RoomPlayer newLeaderClient)
        {
            ChangeLeaderClientOperationResponse waitingResult = null;

            changeLeaderClient(room, newLeaderClient, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        /// <summary>
        /// Send destroy the game object room in eunView
        /// If request success, the EUNManagerBehaviour.OnEUNDestroyGameObjectRoom() and EUNBehaviour.OnEUNDestroyGameObjectRoom() callback
        /// </summary>
        /// <param name="eunView">The eunView need destroy</param>
        /// <param name="onResponse"></param>
        public static void destroyGameObjectRoom(this Room room, EUNView eunView, Action<DestroyGameObjectRoomRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.destroyGameObjectRoom(eunView.roomGameObject.objectId, onResponse);
        }

        public static async Task<DestroyGameObjectRoomRoomOperationResponse> destroyGameObjectRoomAsync(this Room room, EUNView eunView)
        {
            DestroyGameObjectRoomRoomOperationResponse waitingResult = null;

            destroyGameObjectRoom(room, eunView, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

        public static void chatRoom(this Room room, string message, Action<ChatRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.chatRoom(message, onResponse);
        }

        public static async Task<ChatRoomOperationResponse> chatRoomAsync(this Room room, string message)
        {
            ChatRoomOperationResponse waitingResult = null;

            chatRoom(room, message, response => waitingResult = response);

            while (waitingResult == null)
                await Task.Yield();

            return waitingResult;
        }

    }

}
