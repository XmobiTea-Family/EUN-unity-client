namespace EUN.Entity.Response
{
    using EUN.Common;
    using EUN.Constant;

    public class SubscriberChatLobbyOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public SubscriberChatLobbyOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
#if EUN
            if (!HasError)
            {
                Success = true;
            }
#endif
        }
    }
}