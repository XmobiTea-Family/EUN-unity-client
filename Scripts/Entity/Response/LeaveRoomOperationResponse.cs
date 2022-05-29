namespace XmobiTea.EUN.Entity.Response
{
    public class LeaveRoomOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public LeaveRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                Success = true;
            }
        }
    }
}
