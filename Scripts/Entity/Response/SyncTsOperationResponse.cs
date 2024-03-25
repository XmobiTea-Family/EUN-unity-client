namespace XmobiTea.EUN.Entity.Response
{
    using XmobiTea.EUN.Constant;

    public class SyncTsOperationResponse : CustomOperationResponse
    {
        public long serverTimeStamp { get; private set; }

        public SyncTsOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                var parameters = operationResponse.getParameters();

                this.serverTimeStamp = parameters.getLong(ParameterCode.Ts);
            }
        }

    }

}
