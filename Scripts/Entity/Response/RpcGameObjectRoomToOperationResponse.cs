namespace XmobiTea.EUN.Entity.Response
{
    public class RpcGameObjectRoomToOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public RpcGameObjectRoomToOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
