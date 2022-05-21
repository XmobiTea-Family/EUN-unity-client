namespace XmobiTea.EUN.Extension
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;
    using XmobiTea.EUN.Entity;
    using XmobiTea.EUN.Entity.Response;

    using System;

    public static partial class EUNNetworkExtension
    {
        #region Extension

        public static void JoinLobby(this LobbyStats lobbyStats, bool subscriberChat, Action<JoinLobbyOperationResponse> onResponse = null)
        {
            EUNNetwork.JoinLobby(lobbyStats.LobbyId, subscriberChat, response => {
                onResponse?.Invoke(response);
            });
        }

        public static void ChangeLeaderClient(this RoomPlayer newLeaderClient, Action<ChangeLeaderClientOperationResponse> onResponse = null)
        {
            EUNNetwork.ChangeLeaderClient(newLeaderClient.PlayerId, onResponse);
        }

        public static void ChangeCustomProperties(this RoomPlayer roomPlayer, EUNHashtable customPlayerProperties, Action<ChangePlayerCustomPropertiesOperationResponse> onResponse = null)
        {
            EUNNetwork.ChangePlayerCustomProperties(roomPlayer.PlayerId, customPlayerProperties, onResponse);
        }

        public static void TransferGameObjectRoom(this RoomPlayer roomPlayer, EUNView eunView, Action<TransferGameObjectRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.TransferGameObjectRoom(eunView.RoomGameObject.ObjectId, roomPlayer.PlayerId, onResponse);
        }

        public static void TransferGameObjectRoom(this EUNView eunView, RoomPlayer newOwner, Action<TransferGameObjectRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.TransferGameObjectRoom(eunView.RoomGameObject.ObjectId, newOwner.PlayerId, onResponse);
        }

        public static void TransferGameObjectRoom(this EUNView eunView, int newOwnerId, Action<TransferGameObjectRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.TransferGameObjectRoom(eunView.RoomGameObject.ObjectId, newOwnerId, onResponse);
        }

        public static void RPC(this EUNView eunView, EUNTargets targets, EUNRPCCommand command, params object[] rpcData)
        {
            EUNNetwork.RpcGameObjectRoom(targets, eunView.RoomGameObject.ObjectId, (int)command, rpcData);
        }

        public static void DestroyGameObjectRoom(this RoomGameObject roomGameObject, Action<DestroyGameObjectRoomRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.DestroyGameObjectRoom(roomGameObject.ObjectId, onResponse);
        }

        public static void DestroyGameObjectRoom(this EUNView eunView, Action<DestroyGameObjectRoomRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.DestroyGameObjectRoom(eunView.RoomGameObject.ObjectId, onResponse);
        }

        public static void ChangeGameObjectCustomProperties(this RoomGameObject roomGameObject, EUNHashtable customProperties, Action<ChangeGameObjectRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.ChangeGameObjectCustomProperties(roomGameObject.ObjectId, customProperties, onResponse);
        }

        public static void ChangeGameObjectCustomProperties(this EUNView eunView, EUNHashtable customProperties, Action<ChangeGameObjectRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.ChangeGameObjectCustomProperties(eunView.RoomGameObject.ObjectId, customProperties, onResponse);
        }

        public static void JoinRoom(this LobbyRoomStats lobbyRoomStats, string password, Action<JoinRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.JoinRoom(lobbyRoomStats.RoomId, password, onResponse);
        }

        public static void LeaveRoom(this Room room, Action<LeaveRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.LeaveRoom(onResponse);
        }

        public static void ChangeCustomProperties(this Room room, EUNHashtable customRoomProperties, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                .Add(ParameterCode.CustomRoomProperties, customRoomProperties)
                .Build();

            EUNNetwork.ChangeRoomInfo(eunHashtable, onResponse);
        }

        public static void ChangeMaxPlayer(this Room room, int maxPlayer, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                            .Add(ParameterCode.MaxPlayer, maxPlayer)
                            .Build();

            EUNNetwork.ChangeRoomInfo(eunHashtable, onResponse);
        }

        public static void ChangeCustomPropertiesForLobby(this Room room, EUNHashtable customRoomPropertiesForLobby, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                            .Add(ParameterCode.CustomRoomPropertiesForLobby, customRoomPropertiesForLobby)
                            .Build();

            EUNNetwork.ChangeRoomInfo(eunHashtable, onResponse);
        }

        public static void ChangeOpen(this Room room, bool isOpen, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                            .Add(ParameterCode.IsOpen, isOpen)
                            .Build();

            EUNNetwork.ChangeRoomInfo(eunHashtable, onResponse);
        }

        public static void ChangeVisible(this Room room, bool isVisible, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                            .Add(ParameterCode.IsVisible, isVisible)
                            .Build();

            EUNNetwork.ChangeRoomInfo(eunHashtable, onResponse);
        }

        public static void ChangePassword(this Room room, string password, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                            .Add(ParameterCode.Password, password)
                            .Build();

            EUNNetwork.ChangeRoomInfo(eunHashtable, onResponse);
        }

        public static void ChangeTtl(this Room room, int ttl, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var eunHashtable = new EUNHashtable.Builder()
                .Add(ParameterCode.Ttl, ttl)
                .Build();

            EUNNetwork.ChangeRoomInfo(eunHashtable, onResponse);
        }

        public static void CreateGameObjectRoom(this Room room, string prefabPath, object initializeData, object synchronizationData, EUNHashtable customGameObjectProperties, Action<CreateGameObjectRoomOperationResponse> onResponse = null)
        {
            EUNNetwork.CreateGameObjectRoom(prefabPath, initializeData, synchronizationData, customGameObjectProperties, onResponse);
        }

#endregion
    }
}
