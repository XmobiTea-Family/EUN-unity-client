namespace XmobiTea.EUN.Entity.Response
{
    public class TransferOwnerGameObjectRoomOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public TransferOwnerGameObjectRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
