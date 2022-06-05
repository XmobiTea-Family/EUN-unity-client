namespace XmobiTea.EUN.Entity.Response
{
    using XmobiTea.EUN.Constant;

    public class SyncTsOperationResponse : CustomOperationResponse
    {
        public long serverTimeStamp { get; private set; }

        public SyncTsOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (success)
            {
                var parameters = operationResponse.GetParameters();

                serverTimeStamp = parameters.GetLong(ParameterCode.Ts);
            }
        }
    }
}
