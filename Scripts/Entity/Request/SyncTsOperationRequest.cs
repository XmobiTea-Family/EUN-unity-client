namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class SyncTsOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.SyncTs;

        protected override bool Reliable => true;

        public SyncTsOperationRequest(int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {

        }
    }
}
