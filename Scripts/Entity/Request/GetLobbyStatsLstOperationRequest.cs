namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class GetLobbyStatsLstOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.GetLobbyStatsLst;

        protected override bool Reliable => true;

        /// <summary>
        /// GetLobbyStatsLstOperationRequest
        /// </summary>
        /// <param name="skip">The lobby count need skip</param>
        /// <param name="limit">The lobby count need get</param>
        /// <param name="timeout"></param>
        public GetLobbyStatsLstOperationRequest(int skip, int limit, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.Skip, skip)
                .Add(ParameterCode.Limit, limit)
                .Build();
        }
    }
}
