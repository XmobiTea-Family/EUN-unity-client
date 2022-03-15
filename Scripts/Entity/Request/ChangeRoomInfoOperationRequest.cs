﻿namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class ChangeRoomInfoOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.ChangeRoomInfo;

        protected override bool Reliable => true;

        public ChangeRoomInfoOperationRequest(CustomHashtable customHashtable, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                 .Add(ParameterCode.CustomHashtable, customHashtable)
                 .Build();
        }
    }
}