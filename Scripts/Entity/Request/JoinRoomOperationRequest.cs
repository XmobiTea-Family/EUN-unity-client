namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class JoinRoomOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.JoinRoom;

        protected override bool Reliable => true;

        public JoinRoomOperationRequest(int roomId, string password, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.RoomId, roomId)
                .Add(ParameterCode.Password, password)
                .Build();
        }
    }
}