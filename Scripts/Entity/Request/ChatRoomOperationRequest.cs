namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class ChatRoomOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.ChatRoom;

        protected override bool Reliable => false;

        public ChatRoomOperationRequest(string message, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.Message, message)
                .Build();
        }
    }
}
