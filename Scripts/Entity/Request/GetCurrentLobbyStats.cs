namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class GetCurrentLobbyStatsOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.GetCurrentLobbyStats;

        protected override bool reliable => true;

        /// <summary>
        /// GetCurrentLobbyStatsOperationRequest
        /// </summary>
        /// <param name="skip">The room count need skip</param>
        /// <param name="limit">The room count need get</param>
        /// <param name="timeout"></param>
        public GetCurrentLobbyStatsOperationRequest(int skip, int limit, int timeout = OperationRequest.defaultTimeout) : base(timeout)
        {
            this.parameters = new EUNHashtable.Builder()
                .add(ParameterCode.Skip, skip)
                .add(ParameterCode.Limit, limit)
                .build();
        }

    }

}
