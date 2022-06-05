namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class JoinOrCreateRoomOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.JoinOrCreateRoom;

        protected override bool reliable => true;

        /// <summary>
        /// JoinOrCreateRoomOperationRequest
        /// </summary>
        /// <param name="targetExpectedCount">The target expected count in expectedProperties match to can join the room match</param>
        /// <param name="expectedProperties">The expected properties match to join room match</param>
        /// <param name="roomOption">The room option if in this lobby does not contains room match</param>
        /// <param name="timeout"></param>
        public JoinOrCreateRoomOperationRequest(int targetExpectedCount, EUNHashtable expectedProperties, RoomOption roomOption, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.MaxPlayer, roomOption.maxPlayer)
                .Add(ParameterCode.TargetExpectedCount, targetExpectedCount)
                .Add(ParameterCode.ExpectedProperties, expectedProperties)
                .Add(ParameterCode.CustomRoomProperties, roomOption.customRoomProperties)
                .Add(ParameterCode.IsVisible, roomOption.isVisible)
                .Add(ParameterCode.IsOpen, roomOption.isOpen)
                .Add(ParameterCode.CustomRoomPropertiesForLobby, roomOption.customRoomPropertiesForLobby)
                .Add(ParameterCode.Password, roomOption.password)
                .Add(ParameterCode.Ttl, roomOption.ttl)
                .Build();
        }
    }
}
