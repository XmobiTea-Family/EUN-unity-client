namespace XmobiTea.EUN.Entity.Response
{
    public class CreateRoomOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public CreateRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
