namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class JoinRandomRoomOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.JoinRandomRoom;

        protected override bool Reliable => true;

        public JoinRandomRoomOperationRequest(int targetExpectedCount, EUNHashtable expectedProperties, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.TargetExpectedCount, targetExpectedCount)
                .Add(ParameterCode.ExpectedProperties, expectedProperties)
                .Build();
        }
    }
}
