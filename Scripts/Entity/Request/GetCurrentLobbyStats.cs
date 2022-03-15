namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class GetCurrentLobbyStatsOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.GetCurrentLobbyStats;

        protected override bool Reliable => true;

        public GetCurrentLobbyStatsOperationRequest(int skip, int limit, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.Skip, skip)
                .Add(ParameterCode.Limit, limit)
                .Build();
        }
    }
}
