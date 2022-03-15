namespace XmobiTea.EUN.Entity.Response
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public abstract class CustomOperationResponse
    {
        public ReturnCode ReturnCode { get; private set; }
        public string DebugMessage { get; private set; }

        public bool HasError => ReturnCode != ReturnCode.Ok;

        public CustomOperationResponse(OperationResponse operationResponse)
        {
            ReturnCode = operationResponse.GetReturnCode();
            DebugMessage = operationResponse.GetDebugMessage();
        }
    }
}