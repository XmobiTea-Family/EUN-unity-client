namespace XmobiTea.EUN.Entity.Response
{
    public class ChangeLeaderClientOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public ChangeLeaderClientOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                Success = true;
            }
        }
    }
}
