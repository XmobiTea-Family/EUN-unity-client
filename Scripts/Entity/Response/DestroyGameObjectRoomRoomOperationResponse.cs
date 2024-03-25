namespace XmobiTea.EUN.Entity.Response
{
    public class DestroyGameObjectRoomRoomOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public DestroyGameObjectRoomRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
