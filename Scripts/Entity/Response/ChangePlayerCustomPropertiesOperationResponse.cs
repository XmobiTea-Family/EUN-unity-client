namespace XmobiTea.EUN.Entity.Response
{
    using XmobiTea.EUN.Common;

    public class ChangePlayerCustomPropertiesOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public ChangePlayerCustomPropertiesOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                Success = true;
            }
        }
    }
}
