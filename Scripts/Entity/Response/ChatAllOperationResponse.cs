namespace XmobiTea.EUN.Entity.Response
{
    public class ChatAllOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public ChatAllOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
