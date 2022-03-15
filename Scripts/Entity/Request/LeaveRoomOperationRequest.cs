namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class LeaveRoomOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.LeaveRoom;

        protected override bool Reliable => true;

        public LeaveRoomOperationRequest(int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {

        }
    }
}