namespace EUN.Entity.Response
{
    using EUN.Common;

    public class JoinOrCreateRoomOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public JoinOrCreateRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
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