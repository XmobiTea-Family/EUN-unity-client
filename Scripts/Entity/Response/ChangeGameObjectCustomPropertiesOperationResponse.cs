namespace EUN.Entity.Response
{
    using EUN.Common;

    public class ChangeGameObjectRoomOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public ChangeGameObjectRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
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