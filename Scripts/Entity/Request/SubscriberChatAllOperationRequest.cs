namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class SubscriberChatAllOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.SubscriberChatAll;

        protected override bool Reliable => true;

        public SubscriberChatAllOperationRequest(bool isSubscribe, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.Subscribe, isSubscribe)
                .Build();
        }
    }
}