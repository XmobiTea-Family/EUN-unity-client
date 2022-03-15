namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

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
