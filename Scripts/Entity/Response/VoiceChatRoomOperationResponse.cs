namespace XmobiTea.EUN.Entity.Response
{
    public class VoiceChatRoomOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public VoiceChatRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
