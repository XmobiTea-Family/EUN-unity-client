namespace XmobiTea.EUN.Entity.Response
{
    public class LeaveLobbyOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public LeaveLobbyOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
