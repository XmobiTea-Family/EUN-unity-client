namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class JoinOrCreateRoomOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.JoinOrCreateRoom;

        protected override bool Reliable => true;

        public JoinOrCreateRoomOperationRequest(int targetExpectedCount, CustomHashtable expectedProperties, RoomOption roomOption, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
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