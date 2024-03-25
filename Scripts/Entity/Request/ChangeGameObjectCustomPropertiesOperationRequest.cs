namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class ChangeGameObjectCustomPropertiesOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.ChangeGameObjectCustomProperties;

        protected override bool reliable => true;

        /// <summary>
        /// ChangeGameObjectCustomPropertiesOperationRequest
        /// </summary>
        /// <param name="objectId">The room game object need change</param>
        /// <param name="customGameObjectProperties">The custom properties need change</param>
        public ChangeGameObjectCustomPropertiesOperationRequest(int objectId, EUNHashtable customGameObjectProperties, int timeout = OperationRequest.defaultTimeout) : base(timeout)
        {
            this.parameters = new EUNHashtable.Builder()
                .add(ParameterCode.ObjectId, objectId)
                .add(ParameterCode.CustomGameObjectProperties, customGameObjectProperties)
                .build();
        }

    }

}
