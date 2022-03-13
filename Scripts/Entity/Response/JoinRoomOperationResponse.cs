namespace EUN.Entity.Response
{
    using EUN.Common;

    public class JoinRoomOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public JoinRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
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