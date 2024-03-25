namespace XmobiTea.EUN.Entity.Response
{
    public class ChatRoomOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public ChatRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
