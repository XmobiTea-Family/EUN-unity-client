namespace XmobiTea.EUN.Entity.Response
{
    using XmobiTea.EUN.Constant;

    public class GetLobbyStatsLstOperationResponse : CustomOperationResponse
    {
        public LobbyStats[] lobbyStatss { get; private set; }

        public GetLobbyStatsLstOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                var parameters = operationResponse.getParameters();

                var array = parameters.getEUNArray(ParameterCode.Data);

                var array0 = array.getEUNArray(0);
                this.lobbyStatss = new LobbyStats[array0.count()];
                for (var i = 0; i < this.lobbyStatss.Length; i++) this.lobbyStatss[i] = new LobbyStats(array0.getEUNArray(i));
            }
        }

    }

}
