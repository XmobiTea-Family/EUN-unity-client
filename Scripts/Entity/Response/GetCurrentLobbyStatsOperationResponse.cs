namespace EUN.Entity.Response
{
    using com.tvd12.ezyfoxserver.client.entity;

    using EUN.Common;
    using EUN.Constant;

    public class GetCurrentLobbyStatsOperationResponse : CustomOperationResponse
    {
        public LobbyStats LobbyStats { get; private set; }
        public LobbyRoomStats[] LobbyRoomStatss { get; private set; }

        public GetCurrentLobbyStatsOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                var parameters = operationResponse.GetParameters();

                var array = parameters.GetEzyArray(ParameterCode.Data);

                var array0 = array.get<EzyArray>(0);
                LobbyStats = new LobbyStats(array0);

                var array1 = array.get<EzyArray>(1);
                LobbyRoomStatss = new LobbyRoomStats[array1.size()];

                for (var i = 0; i < LobbyRoomStatss.Length; i++)
                {
                    LobbyRoomStatss[i] = new LobbyRoomStats(array1.get<EzyArray>(i));
                }
            }
        }
    }
}