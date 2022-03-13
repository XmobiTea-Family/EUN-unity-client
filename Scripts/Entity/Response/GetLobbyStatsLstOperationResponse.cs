namespace EUN.Entity.Response
{
    using EUN.Common;
    using EUN.Constant;

    public class GetLobbyStatsLstOperationResponse : CustomOperationResponse
    {
        public LobbyStats[] LobbyStatss { get; private set; }

        public GetLobbyStatsLstOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                var parameters = operationResponse.GetParameters();

                var array = parameters.GetCustomArray(ParameterCode.Data);

                var array0 = array.GetCustomArray(0);
                LobbyStatss = new LobbyStats[array0.Count()];
                for (var i = 0; i < LobbyStatss.Length; i++) LobbyStatss[i] = new LobbyStats(array0.GetCustomArray(i));
            }
        }
    }
}