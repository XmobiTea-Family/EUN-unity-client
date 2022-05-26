namespace XmobiTea.EUN.Entity.Response
{
    public class ChangePlayerCustomPropertiesOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public ChangePlayerCustomPropertiesOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                Success = true;
            }
        }
    }
}
