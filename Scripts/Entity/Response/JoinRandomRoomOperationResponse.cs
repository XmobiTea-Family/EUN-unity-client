namespace XmobiTea.EUN.Entity.Response
{
    public class JoinRandomRoomOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public JoinRandomRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
