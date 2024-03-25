namespace XmobiTea.EUN.Entity.Response
{
    using XmobiTea.EUN.Constant;

    public class GetCurrentLobbyStatsOperationResponse : CustomOperationResponse
    {
        public LobbyStats lobbyStats { get; private set; }
        public LobbyRoomStats[] lobbyRoomStatss { get; private set; }

        public GetCurrentLobbyStatsOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                var parameters = operationResponse.getParameters();

                var array = parameters.getEUNArray(ParameterCode.Data);

                var array0 = array.getEUNArray(0);
                this.lobbyStats = new LobbyStats(array0);

                var array1 = array.getEUNArray(1);
                this.lobbyRoomStatss = new LobbyRoomStats[array1.count()];

                for (var i = 0; i < this.lobbyRoomStatss.Length; i++)
                {
                    this.lobbyRoomStatss[i] = new LobbyRoomStats(array1.getEUNArray(i));
                }
            }
        }

    }

}
