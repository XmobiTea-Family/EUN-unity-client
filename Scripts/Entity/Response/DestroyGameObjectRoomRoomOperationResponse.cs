namespace XmobiTea.EUN.Entity.Response
{
    using XmobiTea.EUN.Common;

    public class DestroyGameObjectRoomRoomOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public DestroyGameObjectRoomRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                Success = true;
            }
        }
    }
}
