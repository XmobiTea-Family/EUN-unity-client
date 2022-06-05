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
        public CreateRoomOperationRequest(RoomOption roomOption, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.MaxPlayer, roomOption.maxPlayer)
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
