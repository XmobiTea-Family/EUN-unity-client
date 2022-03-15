namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class ChatLobbyOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.ChatLobby;

        protected override bool Reliable => false;

        public ChatLobbyOperationRequest(string message, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.Message, message)
                .Build();
        }
    }
}
