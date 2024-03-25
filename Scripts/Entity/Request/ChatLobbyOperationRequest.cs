namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class ChatLobbyOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.ChatLobby;

        protected override bool reliable => false;

        /// <summary>
        /// ChatLobbyOperationRequest
        /// </summary>
        /// <param name="message">The message content need send</param>
        /// <param name="timeout"></param>
        public ChatLobbyOperationRequest(string message, int timeout = OperationRequest.defaultTimeout) : base(timeout)
        {
            this.parameters = new EUNHashtable.Builder()
                .add(ParameterCode.Message, message)
                .build();
        }

    }

}
