namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class ChangeLeaderClientOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.ChangeLeaderClient;

        protected override bool Reliable => true;

        /// <summary>
        /// ChangeLeaderClientOperationRequest
        /// </summary>
        /// <param name="leaderClientPlayerId">Leader client player id need change</param>
        /// <param name="timeout"></param>
        public ChangeLeaderClientOperationRequest(int leaderClientPlayerId, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.LeaderClientPlayerId, leaderClientPlayerId)
                .Build();
        }
    }
}
