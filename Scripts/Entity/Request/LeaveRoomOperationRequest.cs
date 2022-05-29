namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class LeaveRoomOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.LeaveRoom;

        protected override bool Reliable => true;

        /// <summary>
        /// LeaveRoomOperationRequest
        /// </summary>
        /// <param name="timeout"></param>
        public LeaveRoomOperationRequest(int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {

        }
    }
}
