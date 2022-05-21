namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class TransferGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.TransferGameObjectRoom;

        protected override bool Reliable => true;

        public TransferGameObjectRoomOperationRequest(int objectId, int ownerId, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Add(ParameterCode.OwnerId, ownerId)
                .Build();
        }
    }
}
