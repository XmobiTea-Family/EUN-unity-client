namespace XmobiTea.EUN.Entity.Response
{
    using XmobiTea.EUN.Constant;

    public class GetCurrentLobbyStatsOperationResponse : CustomOperationResponse
    {
        public LobbyStats lobbyStats { get; private set; }
        public LobbyRoomStats[] lobbyRoomStatss { get; private set; }

        public GetCurrentLobbyStatsOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (success)
            {
                var parameters = operationResponse.GetParameters();

                var array = parameters.GetEUNArray(ParameterCode.Data);

                var array0 = array.GetEUNArray(0);
                lobbyStats = new LobbyStats(array0);

                var array1 = array.GetEUNArray(1);
                lobbyRoomStatss = new LobbyRoomStats[array1.Count()];

                for (var i = 0; i < lobbyRoomStatss.Length; i++)
                {
                    lobbyRoomStatss[i] = new LobbyRoomStats(array1.GetEUNArray(i));
                }
            }
        }
    }
}
