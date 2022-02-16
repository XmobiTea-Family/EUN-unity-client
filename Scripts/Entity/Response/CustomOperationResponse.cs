namespace EUN.Entity.Response
{
    using EUN.Common;
    using EUN.Constant;

    public class CustomOperationResponse
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