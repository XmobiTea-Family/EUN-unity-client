namespace XmobiTea.EUN.Constant
{
    public class ReturnCode
    {
        public const int InvalidRequestParameters = -6;
        public const int OperationTimeout = -5;
        public const int AppNullRequest = -4;
        public const int OperationInvalid = -3;
        public const int InternalServerError = -2;
        public const int NotOk = -1;

        public const int Ok = 0;
        public const int RoomFull = 10;
        public const int RoomClosed = 11;
        public const int LobbyFull = 12;
        public const int RoomNotFound = 14;
        public const int RoomPasswordWrong = 15;

        public const int UserInRoom = 16;
    }
}