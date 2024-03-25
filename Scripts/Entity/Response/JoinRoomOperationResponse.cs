namespace XmobiTea.EUN.Entity.Response
{
    public class JoinRoomOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public JoinRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
