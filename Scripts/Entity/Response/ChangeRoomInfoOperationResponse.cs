namespace XmobiTea.EUN.Entity.Response
{
    public class ChangeRoomInfoOperationResponse : CustomOperationResponse
    {
        public bool Success { get; private set; }

        public ChangeRoomInfoOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!HasError)
            {
                Success = true;
            }
        }
    }
}
