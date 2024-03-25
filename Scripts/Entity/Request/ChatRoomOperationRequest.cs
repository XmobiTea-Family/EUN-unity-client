namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class ChatRoomOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.ChatRoom;

        protected override bool reliable => false;

        /// <summary>
        /// ChatRoomOperationRequest
        /// </summary>
        /// <param name="message">The message content need send</param>
        /// <param name="timeout"></param>
        public ChatRoomOperationRequest(string message, int timeout = OperationRequest.defaultTimeout) : base(timeout)
        {
            this.parameters = new EUNHashtable.Builder()
                .add(ParameterCode.Message, message)
                .build();
        }

    }

}
