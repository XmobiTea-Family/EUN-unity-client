namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class ChatAllOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.ChatAll;

        protected override bool Reliable => false;

        /// <summary>
        /// ChatAllOperationRequest
        /// </summary>
        /// <param name="message">The message content need send</param>
        /// <param name="timeout"></param>
        public ChatAllOperationRequest(string message, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.Message, message)
                .Build();
        }
    }
}
