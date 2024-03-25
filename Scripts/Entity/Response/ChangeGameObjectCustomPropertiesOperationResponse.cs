namespace XmobiTea.EUN.Entity.Response
{
    public class ChangeGameObjectRoomOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public ChangeGameObjectRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
