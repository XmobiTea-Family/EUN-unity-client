namespace XmobiTea.EUN.Constant
{
    public partial class OperationCode
    {
        public const int SyncTs = 0;
        public const int GetLobbyStatsLst = 1;
        public const int GetCurrentLobbyStats = 2;
        public const int JoinLobby = 3;
        public const int LeaveLobby = 4;
        public const int ChatAll = 5;
        public const int ChatLobby = 6;
        public const int ChatRoom = 7;
        public const int CreateRoom = 8;
        public const int JoinOrCreateRoom = 9;
        public const int JoinRoom = 10;
        public const int LeaveRoom = 11;

        public const int ChangeLeaderClient = 12;
        public const int ChangeRoomInfo = 13;
        public const int SubscriberChatAll = 14;
        public const int SubscriberChatLobby = 15;
        public const int ChangePlayerCustomProperties = 16;

        public const int RpcGameObjectRoom = 17;
        public const int CreateGameObjectRoom = 18;
        public const int DestroyGameObjectRoom = 19;
        public const int SynchronizationDataGameObjectRoom = 20;
        public const int TransferGameObjectRoom = 21;
        public const int VoiceChat = 22;
        public const int RpcGameObjectRoomTo = 23;

        public const int ChangeGameObjectCustomProperties = 24;
    }
}