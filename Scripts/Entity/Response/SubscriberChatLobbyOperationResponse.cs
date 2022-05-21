namespace XmobiTea.EUN.Entity.Response
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class SubscriberChatLobbyOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public SubscriberChatLobbyOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                Success = true;
            }
        }
    }
}
