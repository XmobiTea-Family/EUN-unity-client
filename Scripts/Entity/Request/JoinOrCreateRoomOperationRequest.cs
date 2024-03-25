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
        public JoinOrCreateRoomOperationRequest(int targetExpectedCount, EUNHashtable expectedProperties, RoomOption roomOption, int timeout = OperationRequest.defaultTimeout) : base(timeout)
        {
            this.parameters = new EUNHashtable.Builder()
                .add(ParameterCode.MaxPlayer, roomOption.maxPlayer)
                .add(ParameterCode.TargetExpectedCount, targetExpectedCount)
                .add(ParameterCode.ExpectedProperties, expectedProperties)
                .add(ParameterCode.CustomRoomProperties, roomOption.customRoomProperties)
                .add(ParameterCode.IsVisible, roomOption.isVisible)
                .add(ParameterCode.IsOpen, roomOption.isOpen)
                .add(ParameterCode.CustomRoomPropertiesForLobby, roomOption.customRoomPropertiesForLobby)
                .add(ParameterCode.Password, roomOption.password)
                .add(ParameterCode.Ttl, roomOption.ttl)
                .build();
        }

    }

}
