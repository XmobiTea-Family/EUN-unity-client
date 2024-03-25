namespace XmobiTea.EUN.Entity.Response
{
    public class ChangeLeaderClientOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public ChangeLeaderClientOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
