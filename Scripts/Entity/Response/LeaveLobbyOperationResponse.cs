namespace XmobiTea.EUN.Entity.Response
{
    public class LeaveLobbyOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public LeaveLobbyOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                Success = true;
            }
        }
    }
}
