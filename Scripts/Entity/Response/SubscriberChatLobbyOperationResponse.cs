namespace XmobiTea.EUN.Entity.Response
{
    public class SubscriberChatLobbyOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public SubscriberChatLobbyOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
