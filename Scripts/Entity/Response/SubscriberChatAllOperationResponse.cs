namespace XmobiTea.EUN.Entity.Response
{
    public class SubscriberChatAllOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public SubscriberChatAllOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
