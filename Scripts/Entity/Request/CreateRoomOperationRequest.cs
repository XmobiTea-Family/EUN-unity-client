﻿namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class CreateRoomOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.CreateRoom;

        protected override bool Reliable => true;

        public CreateRoomOperationRequest(RoomOption roomOption, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.MaxPlayer, roomOption.MaxPlayer)
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
