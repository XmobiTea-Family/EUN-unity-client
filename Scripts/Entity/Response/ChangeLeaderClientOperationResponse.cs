namespace EUN.Entity.Response
{
    using EUN.Common;

    public class ChangeLeaderClientOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public ChangeLeaderClientOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
#if EUN
            if (!HasError)
            {
                Success = true;
            }
#endif
        }
    }
}