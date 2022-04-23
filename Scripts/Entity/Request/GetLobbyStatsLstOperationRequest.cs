namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class GetLobbyStatsLstOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.GetLobbyStatsLst;

        protected override bool Reliable => true;

        public GetLobbyStatsLstOperationRequest(int skip, int limit, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.Skip, skip)
                .Add(ParameterCode.Limit, limit)
                .Build();
        }
    }
}
