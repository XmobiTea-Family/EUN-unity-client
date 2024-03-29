﻿namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class GetLobbyStatsLstOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.GetLobbyStatsLst;

        protected override bool reliable => true;

        /// <summary>
        /// GetLobbyStatsLstOperationRequest
        /// </summary>
        /// <param name="skip">The lobby count need skip</param>
        /// <param name="limit">The lobby count need get</param>
        /// <param name="timeout"></param>
        public GetLobbyStatsLstOperationRequest(int skip, int limit, int timeout = OperationRequest.defaultTimeout) : base(timeout)
        {
            this.parameters = new EUNHashtable.Builder()
                .add(ParameterCode.Skip, skip)
                .add(ParameterCode.Limit, limit)
                .build();
        }

    }

}
