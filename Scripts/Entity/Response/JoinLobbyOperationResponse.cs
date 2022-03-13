namespace EUN.Entity.Response
{
    using EUN.Common;

    public class JoinLobbyOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public JoinLobbyOperationResponse(OperationResponse operationResponse) : base(operationResponse)
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