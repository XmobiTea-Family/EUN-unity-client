namespace XmobiTea.EUN.Entity.Response
{
    public class JoinOrCreateRoomOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public JoinOrCreateRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
