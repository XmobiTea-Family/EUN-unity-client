namespace EUN.Entity.Response
{
    using EUN.Common;

    public class SubscriberChatAllOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public SubscriberChatAllOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                Success = true;
            }
        }
    }
}