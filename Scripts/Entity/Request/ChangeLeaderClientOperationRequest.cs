namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class ChangeLeaderClientOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.ChangeLeaderClient;

        protected override bool Reliable => true;

        public ChangeLeaderClientOperationRequest(int leaderClientPlayerId, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.LeaderClientPlayerId, leaderClientPlayerId)
                .Build();
        }
    }
}
