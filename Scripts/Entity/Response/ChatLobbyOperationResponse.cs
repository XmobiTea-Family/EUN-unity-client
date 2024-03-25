namespace XmobiTea.EUN.Entity.Response
{
    public class ChatLobbyOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public ChatLobbyOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
