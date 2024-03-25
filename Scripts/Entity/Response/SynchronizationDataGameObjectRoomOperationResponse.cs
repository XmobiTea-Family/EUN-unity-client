namespace XmobiTea.EUN.Entity.Response
{
    public class SynchronizationDataGameObjectRoomOperationResponse : CustomOperationResponse
    {
        public bool success { get; private set; }

        public SynchronizationDataGameObjectRoomOperationResponse(OperationResponse operationResponse) : base(operationResponse)
        {
            if (!this.hasError)
            {
                this.success = true;
            }
        }

    }

}
