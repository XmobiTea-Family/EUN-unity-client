namespace EUN.Constant
{
    public enum ReturnCode
    {
        InvalidRequestParameters = -6,
        OperationTimeout = -5,
        AppNullRequest = -4,
        OperationInvalid = -3,
        InternalServerError = -2,
        NotOk = -1,

        Ok = 0,
        RoomFull = 10,
        RoomClosed = 11,
        LobbyFull = 12,
        RoomNotFound = 14,
        RoomPasswordWrong = 15,

        UserInRoom = 16,
    }
}