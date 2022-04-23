namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class SyncTsOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.SyncTs;

        protected override bool Reliable => true;

        public SyncTsOperationRequest(int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {

        }
    }
}
