namespace XmobiTea.EUN.Entity.Response
{
    public class ChangePlayerCustomPropertiesOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public ChangePlayerCustomPropertiesOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
