namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class ChatAllOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.ChatAll;

        protected override bool Reliable => false;

        public ChatAllOperationRequest(string message, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.Message, message)
                .Build();
        }
    }
}
