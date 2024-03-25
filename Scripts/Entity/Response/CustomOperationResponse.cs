namespace XmobiTea.EUN.Entity.Response
{
    public abstract class CustomOperationResponse
    {
        public int returnCode { get; private set; }
        public string debugMessage { get; private set; }

        public bool hasError => this.returnCode != Constant.ReturnCode.Ok;

        public CustomOperationResponse(OperationResponse operationResponse)
        {
            this.returnCode = operationResponse.getReturnCode();
            this.debugMessage = operationResponse.getDebugMessage();
        }

    }

}
