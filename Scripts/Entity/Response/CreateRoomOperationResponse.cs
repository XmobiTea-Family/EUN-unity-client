namespace EUN.Entity.Response
{
    using EUN.Common;

    public class CreateRoomOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public CreateRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                Success = true;
            }
        }
    }
}