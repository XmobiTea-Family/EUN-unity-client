namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class JoinRoomOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.JoinRoom;

        protected override bool reliable => true;

        /// <summary>
        /// JoinRoomOperationRequest
        /// </summary>
        /// <param name="roomId">The room id need join</param>
        /// <param name="password">The password of room, if this room has password</param>
        /// <param name="timeout"></param>
        public JoinRoomOperationRequest(int roomId, string password, int timeout = OperationRequest.defaultTimeout) : base(timeout)
        {
            this.parameters = new EUNHashtable.Builder()
                .add(ParameterCode.RoomId, roomId)
                .add(ParameterCode.Password, password)
                .build();
        }

    }

}
