namespace EUN.Entity.Response
{
    using EUN.Common;

    public class DestroyGameObjectRoomRoomOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public DestroyGameObjectRoomRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
#if EUN
            if (!HasError)
            {
                Success = true;
            }
#endif
        }
    }
}