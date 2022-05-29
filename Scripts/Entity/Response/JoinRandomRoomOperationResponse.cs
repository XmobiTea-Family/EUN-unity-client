namespace XmobiTea.EUN.Entity.Response
{
    public class JoinRandomRoomOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public JoinRandomRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                Success = true;
            }
        }
    }
}
