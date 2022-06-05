namespace XmobiTea.EUN.Entity.Response
{
    using XmobiTea.EUN.Constant;

    public class GetLobbyStatsLstOperationResponse : CustomOperationResponse
    {
        public LobbyStats[] lobbyStatss { get; private set; }

        public GetLobbyStatsLstOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (success)
            {
                var parameters = operationResponse.GetParameters();

                var array = parameters.GetEUNArray(ParameterCode.Data);

                var array0 = array.GetEUNArray(0);
                lobbyStatss = new LobbyStats[array0.Count()];
                for (var i = 0; i < lobbyStatss.Length; i++) lobbyStatss[i] = new LobbyStats(array0.GetEUNArray(i));
            }
        }
    }
}
