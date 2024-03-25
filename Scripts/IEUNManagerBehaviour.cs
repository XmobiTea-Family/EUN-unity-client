namespace XmobiTea.EUN
{
#if EUN_USING_ONLINE
    using com.tvd12.ezyfoxserver.client.constant;
#else
    using XmobiTea.EUN.Entity.Support;
#endif

    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Entity;

    public interface IEUNManagerBehaviour
    {
        void onEUNCustomPlayerPropertiesChange(RoomPlayer player, EUNHashtable customPropertiesChange);

        void onEUNCustomRoomPropertiesChange(EUNHashtable customPropertiesChange);

        void onEUNCustomGameObjectPropertiesChange(RoomGameObject roomGameObject, EUNHashtable customPropertiesChange);

        void onEUNDestroyGameObjectRoom(RoomGameObject roomGameObject);

        void onEUNZoneConnected();

        void onEUNConnected();

        void onEUNLoginError();

        void onEUNConnectionFailure(EzyConnectionFailedReason reason);

        void onEUNDisconnected(EzyDisconnectReason reason);

        void onEUNJoinLobby();

        void onEUNLeftLobby();

        void onEUNJoinRoom();

        void onEUNLeaderClientChange(RoomPlayer newLeaderClientPlayer);

        void onEUNLeftRoom();

        void onEUNOtherPlayerJoinRoom(RoomPlayer player);

        void onEUNOtherPlayerLeftRoom(RoomPlayer player);

        void onEUNReceiveChatAll(ChatMessage message);

        void onEUNReceiveChatLobby(ChatMessage message);

        void onEUNReceiveChatRoom(ChatMessage message, RoomPlayer sender);

        void onEUNRoomInfoChange(EUNHashtable customPropertiesChange);

        EUNView onEUNViewNeedCreate(RoomGameObject roomGameObject);

        void onEUNTransferOwnerGameObject(RoomGameObject roomGameObject, RoomPlayer newOwner);

    }

}
