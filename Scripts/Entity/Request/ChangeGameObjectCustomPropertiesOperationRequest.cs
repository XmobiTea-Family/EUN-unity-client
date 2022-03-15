namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class ChangeGameObjectCustomPropertiesOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.ChangeGameObjectCustomProperties;

        protected override bool Reliable => true;

        public ChangeGameObjectCustomPropertiesOperationRequest(int objectId, CustomHashtable customGameObjectProperties, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Add(ParameterCode.CustomGameObjectProperties, customGameObjectProperties)
                .Build();
        }
    }
}
