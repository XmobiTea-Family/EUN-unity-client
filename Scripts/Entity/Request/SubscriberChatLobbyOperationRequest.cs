namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class SubscriberChatLobbyOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.SubscriberChatLobby;

        protected override bool Reliable => true;

        public SubscriberChatLobbyOperationRequest(bool isSubscribe, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.Subscribe, isSubscribe)
                .Build();
        }
    }
}