namespace XmobiTea.EUN.Entity.Response
{
    public class JoinLobbyOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public JoinLobbyOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
