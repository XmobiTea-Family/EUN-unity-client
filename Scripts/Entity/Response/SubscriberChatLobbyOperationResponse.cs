namespace EUN.Entity.Response
{
    using EUN.Common;
    using EUN.Constant;

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