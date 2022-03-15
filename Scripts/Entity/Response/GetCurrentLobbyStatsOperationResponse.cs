namespace XmobiTea.EUN.Entity.Response
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class GetCurrentLobbyStatsOperationResponse : CustomOperationResponse
    {
        public LobbyStats LobbyStats { get; private set; }
        public LobbyRoomStats[] LobbyRoomStatss { get; private set; }

        public GetCurrentLobbyStatsOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                var parameters = operationResponse.GetParameters();

                var array = parameters.GetEUNArray(ParameterCode.Data);

                var array0 = array.GetEUNArray(0);
                LobbyStats = new LobbyStats(array0);

                var array1 = array.GetEUNArray(1);
                LobbyRoomStatss = new LobbyRoomStats[array1.Count()];

                for (var i = 0; i < LobbyRoomStatss.Length; i++)
                {
                    LobbyRoomStatss[i] = new LobbyRoomStats(array1.GetEUNArray(i));
                }
            }
        }
    }
}