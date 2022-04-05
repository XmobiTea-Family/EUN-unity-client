namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class JoinLobbyOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.JoinLobby;

        protected override bool Reliable => true;

        public JoinLobbyOperationRequest(int lobbyId, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.LobbyId, lobbyId)
                .Build();
        }
    }
}
