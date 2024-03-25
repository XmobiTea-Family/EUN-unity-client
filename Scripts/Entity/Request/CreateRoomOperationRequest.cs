namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class CreateRoomOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.CreateRoom;

        protected override bool reliable => true;

        /// <summary>
        /// CreateRoomOperationRequest
        /// </summary>
        /// <param name="roomOption">The room option for new room</param>
        /// <param name="timeout"></param>
        public CreateRoomOperationRequest(RoomOption roomOption, int timeout = OperationRequest.defaultTimeout) : base(timeout)
        {
            this.parameters = new EUNHashtable.Builder()
                .add(ParameterCode.MaxPlayer, roomOption.maxPlayer)
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
