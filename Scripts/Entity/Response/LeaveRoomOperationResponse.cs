namespace XmobiTea.EUN.Entity.Response
{
    public class LeaveRoomOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public LeaveRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
