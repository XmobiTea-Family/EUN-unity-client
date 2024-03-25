namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class SyncTsOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.SyncTs;

        protected override bool reliable => true;

        /// <summary>
        /// SyncTsOperationRequest
        /// </summary>
        /// <param name="timeout"></param>
        public SyncTsOperationRequest(int timeout = OperationRequest.defaultTimeout) : base(timeout)
        {
            this.parameters = null;
        }

    }

}
