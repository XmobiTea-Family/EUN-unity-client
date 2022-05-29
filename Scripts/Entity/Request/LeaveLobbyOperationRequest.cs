namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class LeaveLobbyOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.LeaveLobby;

        protected override bool Reliable => true;

        /// <summary>
        /// LeaveLobbyOperationRequest
        /// </summary>
        /// <param name="timeout"></param>
        public LeaveLobbyOperationRequest(int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {

        }
    }
}
