namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class LeaveLobbyOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.LeaveLobby;

        protected override bool Reliable => true;

        public LeaveLobbyOperationRequest(int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {

        }
    }
}
