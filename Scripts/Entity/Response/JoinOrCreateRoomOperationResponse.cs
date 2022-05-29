namespace XmobiTea.EUN.Entity.Response
{
    public class JoinOrCreateRoomOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public JoinOrCreateRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                Success = true;
            }
        }
    }
}
