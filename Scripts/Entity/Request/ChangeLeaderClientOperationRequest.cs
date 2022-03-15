namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class ChangeLeaderClientOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.ChangeLeaderClient;

        protected override bool Reliable => true;

        public ChangeLeaderClientOperationRequest(int leaderClientPlayerId, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.LeaderClientPlayerId, leaderClientPlayerId)
                .Build();
        }
    }
}
