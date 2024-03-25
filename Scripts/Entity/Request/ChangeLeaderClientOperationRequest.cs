namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class ChangeLeaderClientOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.ChangeLeaderClient;

        protected override bool reliable => true;

        /// <summary>
        /// ChangeLeaderClientOperationRequest
        /// </summary>
        /// <param name="leaderClientPlayerId">Leader client player id need change</param>
        /// <param name="timeout"></param>
        public ChangeLeaderClientOperationRequest(int leaderClientPlayerId, int timeout = OperationRequest.defaultTimeout) : base(timeout)
        {
            this.parameters = new EUNHashtable.Builder()
                .add(ParameterCode.LeaderClientPlayerId, leaderClientPlayerId)
                .build();
        }

    }

}
