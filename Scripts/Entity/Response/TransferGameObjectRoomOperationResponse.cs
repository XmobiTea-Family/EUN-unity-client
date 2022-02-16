namespace EUN.Entity.Response
{
    using EUN.Common;

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