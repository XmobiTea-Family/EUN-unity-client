namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class SubscriberChatLobbyOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.SubscriberChatLobby;

        protected override bool Reliable => true;

        /// <summary>
        /// SubscriberChatLobbyOperationRequest
        /// </summary>
        /// <param name="isSubscribe">Subsribe chat</param>
        /// <param name="timeout"></param>
        public SubscriberChatLobbyOperationRequest(bool isSubscribe, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.Subscribe, isSubscribe)
                .Build();
        }
    }
}
