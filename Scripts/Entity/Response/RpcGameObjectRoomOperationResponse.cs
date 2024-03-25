namespace XmobiTea.EUN.Entity.Response
{
    public class RpcGameObjectRoomOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public RpcGameObjectRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
