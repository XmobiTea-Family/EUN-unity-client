namespace XmobiTea.EUN.Entity.Response
{
    public abstract class CustomOperationResponse
    {
        public int ReturnCode { get; private set; }
        public string DebugMessage { get; private set; }

        public bool HasError => ReturnCode != Constant.ReturnCode.Ok;

        public CustomOperationResponse(OperationResponse operationResponse)
        {
            ReturnCode = operationResponse.GetReturnCode();
            DebugMessage = operationResponse.GetDebugMessage();
        }
    }
}
