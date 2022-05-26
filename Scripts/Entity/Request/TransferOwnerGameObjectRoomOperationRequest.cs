namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class TransferOwnerGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.TransferOwnerGameObjectRoom;

        protected override bool Reliable => true;

        public TransferOwnerGameObjectRoomOperationRequest(int objectId, int ownerId, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Add(ParameterCode.OwnerId, ownerId)
                .Build();
        }
    }
}
