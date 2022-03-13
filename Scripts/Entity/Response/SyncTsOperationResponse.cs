namespace EUN.Entity.Response
{
    using EUN.Common;
    using EUN.Constant;

    public class SyncTsOperationResponse : CustomOperationResponse
    {
        public long ServerTimeStamp { get; private set; }

        public SyncTsOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
#if EUN
            if (!HasError)
            {
                var parameters = operationResponse.GetParameters();

                ServerTimeStamp = parameters.GetLong(ParameterCode.Ts);
            }
#endif
        }
    }
}