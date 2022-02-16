namespace EUN.Entity.Response
{
    using com.tvd12.ezyfoxserver.client.entity;

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

                var array = parameters.GetEzyArray(ParameterCode.Data);

                var array0 = array.get<EzyArray>(0);
                LobbyStatss = new LobbyStats[array0.size()];
                for (var i = 0; i < LobbyStatss.Length; i++) LobbyStatss[i] = new LobbyStats(array0.get<EzyArray>(i));
            }
        }
    }
}