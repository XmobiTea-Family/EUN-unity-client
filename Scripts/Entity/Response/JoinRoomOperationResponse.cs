namespace XmobiTea.EUN.Entity.Response
{
    public class JoinRoomOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public JoinRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                Success = true;
            }
        }
    }
}
