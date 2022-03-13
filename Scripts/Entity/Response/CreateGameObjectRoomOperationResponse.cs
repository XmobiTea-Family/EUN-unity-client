namespace EUN.Entity.Response
{
    using EUN.Common;

    public class CreateGameObjectRoomOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public CreateGameObjectRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                Success = true;
            }
        }
    }
}