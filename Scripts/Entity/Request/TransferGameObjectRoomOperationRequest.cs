namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class TransferGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.TransferGameObjectRoom;

        protected override bool Reliable => true;

        public TransferGameObjectRoomOperationRequest(int objectId, int ownerId, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Add(ParameterCode.OwnerId, ownerId)
                .Build();
        }
    }
}
