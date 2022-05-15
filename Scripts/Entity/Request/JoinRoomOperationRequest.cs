namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class JoinRoomOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.JoinRoom;

        protected override bool Reliable => true;

        public JoinRoomOperationRequest(int roomId, string password, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.RoomId, roomId)
                .Add(ParameterCode.Password, password)
                .Build();
        }
    }
}
