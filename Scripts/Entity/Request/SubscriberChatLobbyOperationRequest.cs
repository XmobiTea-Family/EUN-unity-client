namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class SubscriberChatLobbyOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.SubscriberChatLobby;

        protected override bool Reliable => true;

        public SubscriberChatLobbyOperationRequest(bool isSubscribe, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.Subscribe, isSubscribe)
                .Build();
        }
    }
}