﻿namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class JoinRandomRoomOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.JoinRandomRoom;

        protected override bool reliable => true;

        /// <summary>
        /// JoinRandomRoomOperationRequest
        /// </summary>
        /// <param name="targetExpectedCount">The target expected count in expectedProperties match to can join the room match</param>
        /// <param name="expectedProperties">The expected properties match to join room match</param>
        /// <param name="timeout"></param>
        public JoinRandomRoomOperationRequest(int targetExpectedCount, EUNHashtable expectedProperties, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.TargetExpectedCount, targetExpectedCount)
                .Add(ParameterCode.ExpectedProperties, expectedProperties)
                .Build();
        }
    }
}
