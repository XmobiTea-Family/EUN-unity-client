namespace XmobiTea.EUN.Entity.Response
{
    public class CreateGameObjectRoomOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public CreateGameObjectRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
