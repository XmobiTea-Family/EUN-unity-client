namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class ChangeRoomInfoOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.ChangeRoomInfo;

        protected override bool Reliable => true;

        public ChangeRoomInfoOperationRequest(EUNHashtable eunHashtable, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                 .Add(ParameterCode.EUNHashtable, eunHashtable)
                 .Build();
        }
    }
}
