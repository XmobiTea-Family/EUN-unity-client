namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class JoinLobbyOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.JoinLobby;

        protected override bool Reliable => true;

        public JoinLobbyOperationRequest(int lobbyId, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.LobbyId, lobbyId)
                .Build();
        }
    }
}
