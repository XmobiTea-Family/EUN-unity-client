namespace XmobiTea.EUN.Entity.Response
{
    public class ChangeRoomInfoOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public ChangeRoomInfoOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
