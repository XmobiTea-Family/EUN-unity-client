namespace XmobiTea.EUN.Entity.Response
{
    public abstract class CustomOperationResponse
    {
        public int returnCode { get; private set; }
        public string debugMessage { get; private set; }
        public bool hasError => returnCode != Constant.ReturnCode.Ok;
        public bool success { get; private set; }

        public CustomOperationResponse(OperationResponse operationResponse)
        {
            returnCode = operationResponse.GetReturnCode();
            debugMessage = operationResponse.GetDebugMessage();

            if (!hasError) success = true;
        }
    }
}
