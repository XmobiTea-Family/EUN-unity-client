namespace XmobiTea.EUN.Constant
{
    /// <summary>
    /// All return code 
    /// </summary>
    public class ReturnCode
    {
        /// <summary>
        /// The parameters in request invalid
        /// maybe the field require but request does not have field
        /// or the field is number type but request is other type
        /// </summary>
        public const int InvalidRequestParameters = -6;

        /// <summary>
        /// The request waiting response but it timeout
        /// </summary>
        public const int OperationTimeout = -5;

        /// <summary>
        /// The app null request, it mean the operation request send after EUN Server accept this EUN Client
        /// </summary>
        public const int AppNullRequest = -4;

        /// <summary>
        /// The operation request invalid, it mean does not contains operation code
        /// </summary>
        public const int OperationInvalid = -3;

        /// <summary>
        /// the EUN Server handle operation request but it throw an exception in server
        /// </summary>
        public const int InternalServerError = -2;

        /// <summary>
        /// The operation request meet condition not correct to continue handle
        /// </summary>
        public const int NotOk = -1;

        /// <summary>
        /// The operation request send request success
        /// </summary>
        public const int Ok = 0;

        /// <summary>
        /// The room full player
        /// </summary>
        public const int RoomFull = 10;

        /// <summary>
        /// The room has close
        /// </summary>
        public const int RoomClosed = 11;

        /// <summary>
        /// The lobby full 
        /// </summary>
        public const int LobbyFull = 12;

        /// <summary>
        /// The room not found in this lobby
        /// </summary>
        public const int RoomNotFound = 14;

        /// <summary>
        /// The password to join room wrong
        /// </summary>
        public const int RoomPasswordWrong = 15;

        /// <summary>
        /// The user is in the room, you can not join other room before left room
        /// </summary>
        public const int UserInRoom = 16;
    }
}
