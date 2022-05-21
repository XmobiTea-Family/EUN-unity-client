namespace XmobiTea.EUN.Entity.Response
{
    using XmobiTea.EUN.Common;

    public class TransferGameObjectRoomOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public TransferGameObjectRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                Success = true;
            }
        }
    }
}
