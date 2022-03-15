namespace XmobiTea.EUN.Constant
{
    public enum OperationCode : byte
    {
        SyncTs = 0,
        GetLobbyStatsLst = 1,
        GetCurrentLobbyStats = 2,
        JoinLobby = 3,
        LeaveLobby = 4,
        ChatAll = 5,
        ChatLobby = 6,
        ChatRoom = 7,
        CreateRoom = 8,
        JoinOrCreateRoom = 9,
        JoinRoom = 10,
        LeaveRoom = 11,

        ChangeLeaderClient = 12,
        ChangeRoomInfo = 13,
        SubscriberChatAll = 14,
        SubscriberChatLobby = 15,
        ChangePlayerCustomProperties = 16,

        RpcGameObjectRoom = 17,
        CreateGameObjectRoom = 18,
        DestroyGameObjectRoom = 19,
        SynchronizationDataGameObjectRoom = 20,
        TransferGameObjectRoom = 21,
        VoiceChat = 22,
        RpcGameObjectRoomTo = 23,

        ChangeGameObjectCustomProperties = 24,
    }
}