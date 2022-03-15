namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class ChangePlayerCustomPropertiesOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.ChangePlayerCustomProperties;

        protected override bool Reliable => true;

        public ChangePlayerCustomPropertiesOperationRequest(int playerId, CustomHashtable customGameObjectProperties, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.OwnerId, playerId)
                .Add(ParameterCode.CustomPlayerProperties, customGameObjectProperties)
                .Build();
        }
    }
}
