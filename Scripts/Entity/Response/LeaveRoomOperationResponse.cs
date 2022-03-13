namespace EUN.Entity.Response
{
    using EUN.Common;

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