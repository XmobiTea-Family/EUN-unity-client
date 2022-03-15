namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class GetLobbyStatsLstOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.GetLobbyStatsLst;

        protected override bool Reliable => true;

        public GetLobbyStatsLstOperationRequest(int skip, int limit, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.Skip, skip)
                .Add(ParameterCode.Limit, limit)
                .Build();
        }
    }
}
