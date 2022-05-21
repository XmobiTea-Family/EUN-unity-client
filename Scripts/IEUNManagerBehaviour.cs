namespace XmobiTea.EUN
{
#if EUN
    using com.tvd12.ezyfoxserver.client.constant;
#else
    using XmobiTea.EUN.Entity.Support;
#endif

    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Entity;

    public interface IEUNManagerBehaviour
    {
        void OnEUNCustomPlayerPropertiesChange(RoomPlayer player, EUNHashtable customPropertiesChange);

        void OnEUNCustomRoomPropertiesChange(EUNHashtable customPropertiesChange);

        void OnEUNCustomGameObjectPropertiesChange(RoomGameObject roomGameObject, EUNHashtable customPropertiesChange);

        void OnEUNZoneConnected();

        void OnEUNConnected();

        void OnEUNLoginError();

        void OnEUNConnectionFailure(EzyConnectionFailedReason reason);

        void OnEUNDisconnected(EzyDisconnectReason reason);

        void OnEUNJoinLobby();

        void OnEUNLeftLobby();

        void OnEUNJoinRoom();

        void OnEUNLeaderClientChange(RoomPlayer newLeaderClientPlayer);

        void OnEUNLeftRoom();

        void OnEUNOtherPlayerJoinRoom(RoomPlayer player);

        void OnEUNOtherPlayerLeftRoom(RoomPlayer player);

        void OnEUNReceiveChatAll(ChatMessage message);

        void OnEUNReceiveChatLobby(ChatMessage message);

        void OnEUNReceiveChatRoom(ChatMessage message, RoomPlayer sender);

        void OnEUNRoomInfoChange(EUNHashtable customPropertiesChange);

        EUNView OnEUNViewNeedCreate(RoomGameObject roomGameObject);

        void OnEUNTransferOwnerGameObject(RoomGameObject roomGameObject, RoomPlayer newOwner);
    }
}
