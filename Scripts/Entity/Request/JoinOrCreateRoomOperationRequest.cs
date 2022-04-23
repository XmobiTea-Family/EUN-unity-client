namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class JoinOrCreateRoomOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.JoinOrCreateRoom;

        protected override bool Reliable => true;

        public JoinOrCreateRoomOperationRequest(int targetExpectedCount, EUNHashtable expectedProperties, RoomOption roomOption, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.MaxPlayer, roomOption.MaxPlayer)
                .Add(ParameterCode.TargetExpectedCount, targetExpectedCount)
                .Add(ParameterCode.ExpectedProperties, expectedProperties)
                .Add(ParameterCode.CustomRoomProperties, roomOption.CustomRoomProperties)
                .Add(ParameterCode.IsVisible, roomOption.IsVisible)
                .Add(ParameterCode.IsOpen, roomOption.IsOpen)
                .Add(ParameterCode.CustomRoomPropertiesForLobby, roomOption.CustomRoomPropertiesForLobby)
                .Add(ParameterCode.Password, roomOption.Password)
                .Add(ParameterCode.Ttl, roomOption.Ttl)
                .Build();
        }
    }
}