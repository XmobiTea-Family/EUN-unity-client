namespace XmobiTea.EUN.Entity.Response
{
    using XmobiTea.EUN.Common;

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