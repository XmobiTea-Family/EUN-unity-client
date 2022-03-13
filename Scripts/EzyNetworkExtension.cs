namespace EUN.Extension
{
    using EUN.Common;
    using EUN.Constant;
    using EUN.Entity;
    using EUN.Entity.Response;

    using System;

    public static partial class EzyNetworkExtension
    {
        #region Extension

        public static void JoinLobby(this LobbyStats lobbyStats, bool subscriberChat, Action<JoinLobbyOperationResponse> onResponse = null)
        {
            EzyNetwork.JoinLobby(lobbyStats.LobbyId, subscriberChat, response => {
                onResponse?.Invoke(response);
            });
        }

        public static void ChangeLeaderClient(this RoomPlayer newLeaderClient, Action<ChangeLeaderClientOperationResponse> onResponse = null)
        {
            EzyNetwork.ChangeLeaderClient(newLeaderClient.PlayerId, onResponse);
        }

        public static void ChangeCustomProperties(this RoomPlayer roomPlayer, CustomHashtable customPlayerProperties, Action<ChangePlayerCustomPropertiesOperationResponse> onResponse = null)
        {
            EzyNetwork.ChangePlayerCustomProperties(roomPlayer.PlayerId, customPlayerProperties, onResponse);
        }

        public static void TransferGameObjectRoom(this RoomPlayer roomPlayer, EzyView ezyView, Action<TransferGameObjectRoomOperationResponse> onResponse = null)
        {
            EzyNetwork.TransferGameObjectRoom(ezyView.RoomGameObject.ObjectId, roomPlayer.PlayerId, onResponse);
        }

        public static void TransferGameObjectRoom(this EzyView ezyView, RoomPlayer newOwner, Action<TransferGameObjectRoomOperationResponse> onResponse = null)
        {
            EzyNetwork.TransferGameObjectRoom(ezyView.RoomGameObject.ObjectId, newOwner.PlayerId, onResponse);
        }

        public static void TransferGameObjectRoom(this EzyView ezyView, int newOwnerId, Action<TransferGameObjectRoomOperationResponse> onResponse = null)
        {
            EzyNetwork.TransferGameObjectRoom(ezyView.RoomGameObject.ObjectId, newOwnerId, onResponse);
        }

        public static void RPC(this EzyView ezyView, EzyTargets targets, EzyRPCCommand command, params object[] rpcData)
        {
            EzyNetwork.RpcGameObjectRoom(targets, ezyView.RoomGameObject.ObjectId, (int)command, rpcData);
        }

        public static void DestroyGameObjectRoom(this RoomGameObject roomGameObject, Action<DestroyGameObjectRoomRoomOperationResponse> onResponse = null)
        {
            EzyNetwork.DestroyGameObjectRoom(roomGameObject.ObjectId, onResponse);
        }

        public static void DestroyGameObjectRoom(this EzyView ezyView, Action<DestroyGameObjectRoomRoomOperationResponse> onResponse = null)
        {
            EzyNetwork.DestroyGameObjectRoom(ezyView.RoomGameObject.ObjectId, onResponse);
        }

        public static void ChangeGameObjectCustomProperties(this RoomGameObject roomGameObject, CustomHashtable customProperties, Action<ChangeGameObjectRoomOperationResponse> onResponse = null)
        {
            EzyNetwork.ChangeGameObjectCustomProperties(roomGameObject.ObjectId, customProperties, onResponse);
        }

        public static void ChangeGameObjectCustomProperties(this EzyView ezyView, CustomHashtable customProperties, Action<ChangeGameObjectRoomOperationResponse> onResponse = null)
        {
            EzyNetwork.ChangeGameObjectCustomProperties(ezyView.RoomGameObject.ObjectId, customProperties, onResponse);
        }

        public static void JoinRoom(this LobbyRoomStats lobbyRoomStats, string password, Action<JoinRoomOperationResponse> onResponse = null)
        {
            EzyNetwork.JoinRoom(lobbyRoomStats.RoomId, password, onResponse);
        }

        public static void LeaveRoom(this Room room, Action<LeaveRoomOperationResponse> onResponse = null)
        {
            EzyNetwork.LeaveRoom(onResponse);
        }

        public static void ChangeCustomProperties(this Room room, CustomHashtable customRoomProperties, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var customHashtable = new CustomHashtable.Builder()
                .Add(ParameterCode.CustomRoomProperties, customRoomProperties)
                .Build();

            EzyNetwork.ChangeRoomInfo(customHashtable, onResponse);
        }

        public static void ChangeMaxPlayer(this Room room, int maxPlayer, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var customHashtable = new CustomHashtable.Builder()
                            .Add(ParameterCode.MaxPlayer, maxPlayer)
                            .Build();

            EzyNetwork.ChangeRoomInfo(customHashtable, onResponse);
        }

        public static void ChangeCustomPropertiesForLobby(this Room room, CustomHashtable customRoomPropertiesForLobby, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var customHashtable = new CustomHashtable.Builder()
                            .Add(ParameterCode.CustomRoomPropertiesForLobby, customRoomPropertiesForLobby)
                            .Build();

            EzyNetwork.ChangeRoomInfo(customHashtable, onResponse);
        }

        public static void ChangeOpen(this Room room, bool isOpen, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var customHashtable = new CustomHashtable.Builder()
                            .Add(ParameterCode.IsOpen, isOpen)
                            .Build();

            EzyNetwork.ChangeRoomInfo(customHashtable, onResponse);
        }

        public static void ChangeVisible(this Room room, bool isVisible, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var customHashtable = new CustomHashtable.Builder()
                            .Add(ParameterCode.IsVisible, isVisible)
                            .Build();

            EzyNetwork.ChangeRoomInfo(customHashtable, onResponse);
        }

        public static void ChangePassword(this Room room, string password, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var customHashtable = new CustomHashtable.Builder()
                            .Add(ParameterCode.Password, password)
                            .Build();

            EzyNetwork.ChangeRoomInfo(customHashtable, onResponse);
        }

        public static void ChangeTtl(this Room room, int ttl, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
            var customHashtable = new CustomHashtable.Builder()
                .Add(ParameterCode.Ttl, ttl)
                .Build();

            EzyNetwork.ChangeRoomInfo(customHashtable, onResponse);
        }

        public static void CreateGameObjectRoom(this Room room, string prefabPath, object initializeData, object synchronizationData, CustomHashtable customGameObjectProperties, Action<CreateGameObjectRoomOperationResponse> onResponse = null)
        {
            EzyNetwork.CreateGameObjectRoom(prefabPath, initializeData, synchronizationData, customGameObjectProperties, onResponse);
        }

#endregion
    }
}