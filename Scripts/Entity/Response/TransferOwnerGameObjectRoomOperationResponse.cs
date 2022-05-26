namespace XmobiTea.EUN.Entity.Response
{
    public class TransferOwnerGameObjectRoomOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public TransferOwnerGameObjectRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                Success = true;
            }
        }
    }
}
