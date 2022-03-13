namespace EUN.Entity.Response
{
#if EUN
    using com.tvd12.ezyfoxserver.client.entity;
#endif
    using EUN.Common;
    using EUN.Constant;

    public class GetLobbyStatsLstOperationResponse : CustomOperationResponse
    {
        public LobbyStats[] LobbyStatss { get; private set; }

        public GetLobbyStatsLstOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
#if EUN
            if (!HasError)
            {
                var parameters = operationResponse.GetParameters();

                var array = parameters.GetEzyArray(ParameterCode.Data);

                var array0 = array.get<EzyArray>(0);
                LobbyStatss = new LobbyStats[array0.size()];
                for (var i = 0; i < LobbyStatss.Length; i++) LobbyStatss[i] = new LobbyStats(array0.get<EzyArray>(i));
            }
#endif
        }
    }
}