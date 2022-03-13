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
#if EUN
            EzyNetwork.JoinLobby(lobbyStats.LobbyId, subscriberChat, response => {
                onResponse?.Invoke(response);
            });
#endif
        }

        public static void ChangeLeaderClient(this RoomPlayer newLeaderClient, Action<ChangeLeaderClientOperationResponse> onResponse = null)
        {
#if EUN
            EzyNetwork.ChangeLeaderClient(newLeaderClient.PlayerId, onResponse);
#endif
        }

        public static void ChangeCustomProperties(this RoomPlayer roomPlayer, CustomHashtable customPlayerProperties, Action<ChangePlayerCustomPropertiesOperationResponse> onResponse = null)
        {
#if EUN
            EzyNetwork.ChangePlayerCustomProperties(roomPlayer.PlayerId, customPlayerProperties, onResponse);
#endif
        }

        public static void TransferGameObjectRoom(this RoomPlayer roomPlayer, EzyView ezyView, Action<TransferGameObjectRoomOperationResponse> onResponse = null)
        {
#if EUN
            EzyNetwork.TransferGameObjectRoom(ezyView.RoomGameObject.ObjectId, roomPlayer.PlayerId, onResponse);
#endif
        }

        public static void TransferGameObjectRoom(this EzyView ezyView, RoomPlayer newOwner, Action<TransferGameObjectRoomOperationResponse> onResponse = null)
        {
#if EUN
            EzyNetwork.TransferGameObjectRoom(ezyView.RoomGameObject.ObjectId, newOwner.PlayerId, onResponse);
#endif
        }

        public static void TransferGameObjectRoom(this EzyView ezyView, int newOwnerId, Action<TransferGameObjectRoomOperationResponse> onResponse = null)
        {
#if EUN
            EzyNetwork.TransferGameObjectRoom(ezyView.RoomGameObject.ObjectId, newOwnerId, onResponse);
#endif
        }

        public static void RPC(this EzyView ezyView, EzyTargets targets, EzyRPCCommand command, params object[] rpcData)
        {
#if EUN
            EzyNetwork.RpcGameObjectRoom(targets, ezyView.RoomGameObject.ObjectId, (int)command, rpcData);
#endif
        }

        public static void DestroyGameObjectRoom(this RoomGameObject roomGameObject, Action<DestroyGameObjectRoomRoomOperationResponse> onResponse = null)
        {
#if EUN
            EzyNetwork.DestroyGameObjectRoom(roomGameObject.ObjectId, onResponse);
#endif
        }

        public static void DestroyGameObjectRoom(this EzyView ezyView, Action<DestroyGameObjectRoomRoomOperationResponse> onResponse = null)
        {
#if EUN
            EzyNetwork.DestroyGameObjectRoom(ezyView.RoomGameObject.ObjectId, onResponse);
#endif
        }

        public static void ChangeGameObjectCustomProperties(this RoomGameObject roomGameObject, CustomHashtable customProperties, Action<ChangeGameObjectRoomOperationResponse> onResponse = null)
        {
#if EUN
            EzyNetwork.ChangeGameObjectCustomProperties(roomGameObject.ObjectId, customProperties, onResponse);
#endif
        }

        public static void ChangeGameObjectCustomProperties(this EzyView ezyView, CustomHashtable customProperties, Action<ChangeGameObjectRoomOperationResponse> onResponse = null)
        {
#if EUN
            EzyNetwork.ChangeGameObjectCustomProperties(ezyView.RoomGameObject.ObjectId, customProperties, onResponse);
#endif
        }

        public static void JoinRoom(this LobbyRoomStats lobbyRoomStats, string password, Action<JoinRoomOperationResponse> onResponse = null)
        {
#if EUN
            EzyNetwork.JoinRoom(lobbyRoomStats.RoomId, password, onResponse);
#endif
        }

        public static void LeaveRoom(this Room room, Action<LeaveRoomOperationResponse> onResponse = null)
        {
#if EUN
            EzyNetwork.LeaveRoom(onResponse);
#endif
        }

        public static void ChangeCustomProperties(this Room room, CustomHashtable customRoomProperties, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
#if EUN
            var customHashtable = new CustomHashtable.Builder()
                .Add(ParameterCode.CustomRoomProperties, customRoomProperties)
                .Build();

            EzyNetwork.ChangeRoomInfo(customHashtable, onResponse);
#endif
        }

        public static void ChangeMaxPlayer(this Room room, int maxPlayer, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
#if EUN
            var customHashtable = new CustomHashtable.Builder()
                .Add(ParameterCode.MaxPlayer, maxPlayer)
                .Build();

            EzyNetwork.ChangeRoomInfo(customHashtable, onResponse);
#endif
        }

        public static void ChangeCustomPropertiesForLobby(this Room room, CustomHashtable customRoomPropertiesForLobby, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
#if EUN
            var customHashtable = new CustomHashtable.Builder()
                .Add(ParameterCode.CustomRoomPropertiesForLobby, customRoomPropertiesForLobby)
                .Build();

            EzyNetwork.ChangeRoomInfo(customHashtable, onResponse);
#endif
        }

        public static void ChangeOpen(this Room room, bool isOpen, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
#if EUN
            var customHashtable = new CustomHashtable.Builder()
                .Add(ParameterCode.IsOpen, isOpen)
                .Build();

            EzyNetwork.ChangeRoomInfo(customHashtable, onResponse);
#endif
        }

        public static void ChangeVisible(this Room room, bool isVisible, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
#if EUN
            var customHashtable = new CustomHashtable.Builder()
                .Add(ParameterCode.IsVisible, isVisible)
                .Build();

            EzyNetwork.ChangeRoomInfo(customHashtable, onResponse);
#endif
        }

        public static void ChangePassword(this Room room, string password, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
#if EUN
            var customHashtable = new CustomHashtable.Builder()
                .Add(ParameterCode.Password, password)
                .Build();

            EzyNetwork.ChangeRoomInfo(customHashtable, onResponse);
#endif
        }

        public static void ChangeTtl(this Room room, int ttl, Action<ChangeRoomInfoOperationResponse> onResponse = null)
        {
#if EUN
            var customHashtable = new CustomHashtable.Builder()
                .Add(ParameterCode.Ttl, ttl)
                .Build();

            EzyNetwork.ChangeRoomInfo(customHashtable, onResponse);
#endif
        }

        public static void CreateGameObjectRoom(string prefabPath, object initializeData, object synchronizationData, CustomHashtable customGameObjectProperties, Action<CreateGameObjectRoomOperationResponse> onResponse = null)
        {
#if EUN
            EzyNetwork.CreateGameObjectRoom(prefabPath, initializeData, synchronizationData, customGameObjectProperties, onResponse);
#endif
        }

#endregion
    }
}