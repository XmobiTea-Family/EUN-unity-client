namespace XmobiTea.EUN.Entity.Response
{
    using XmobiTea.EUN.Common;

    public class JoinLobbyOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public JoinLobbyOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                Success = true;
            }
        }
    }
}
