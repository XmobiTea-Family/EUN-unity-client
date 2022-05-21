namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class ChangePlayerCustomPropertiesOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.ChangePlayerCustomProperties;

        protected override bool Reliable => true;

        public ChangePlayerCustomPropertiesOperationRequest(int playerId, EUNHashtable customGameObjectProperties, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.OwnerId, playerId)
                .Add(ParameterCode.CustomPlayerProperties, customGameObjectProperties)
                .Build();
        }
    }
}
