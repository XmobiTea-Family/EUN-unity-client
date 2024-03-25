namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class ChangePlayerCustomPropertiesOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.ChangePlayerCustomProperties;

        protected override bool reliable => true;

        /// <summary>
        /// ChangePlayerCustomPropertiesOperationRequest
        /// </summary>
        /// <param name="playerId">The player id need change</param>
        /// <param name="customPlayerProperties">The custom player properties need change</param>
        /// <param name="timeout"></param>
        public ChangePlayerCustomPropertiesOperationRequest(int playerId, EUNHashtable customPlayerProperties, int timeout = OperationRequest.defaultTimeout) : base(timeout)
        {
            this.parameters = new EUNHashtable.Builder()
                .add(ParameterCode.OwnerId, playerId)
                .add(ParameterCode.CustomPlayerProperties, customPlayerProperties)
                .build();
        }

    }

}
