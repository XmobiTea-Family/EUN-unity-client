namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class ChangeGameObjectCustomPropertiesOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.ChangeGameObjectCustomProperties;

        protected override bool Reliable => true;

        public ChangeGameObjectCustomPropertiesOperationRequest(int objectId, EUNHashtable customGameObjectProperties, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Add(ParameterCode.CustomGameObjectProperties, customGameObjectProperties)
                .Build();
        }
    }
}
