namespace EUN.Entity.Response
{
    using EUN.Common;

    public class ChangePlayerCustomPropertiesOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public ChangePlayerCustomPropertiesOperationResponse(OperationResponse operationResponse) : base(operationResponse)
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