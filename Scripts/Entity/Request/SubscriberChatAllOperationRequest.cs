namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class SubscriberChatAllOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.SubscriberChatAll;

        protected override bool reliable => true;

        /// <summary>
        /// SubscriberChatAllOperationRequest
        /// </summary>
        /// <param name="isSubscribe">Subsribe chat</param>
        /// <param name="timeout"></param>
        public SubscriberChatAllOperationRequest(bool isSubscribe, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.Subscribe, isSubscribe)
                .Build();
        }
    }
}
