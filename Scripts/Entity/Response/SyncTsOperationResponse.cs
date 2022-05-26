namespace XmobiTea.EUN.Entity.Response
{
    using XmobiTea.EUN.Constant;

    public class SyncTsOperationResponse : CustomOperationResponse
    {
        public long ServerTimeStamp { get; private set; }

        public SyncTsOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                var parameters = operationResponse.GetParameters();

                ServerTimeStamp = parameters.GetLong(ParameterCode.Ts);
            }
        }
    }
}
