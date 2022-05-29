namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class TransferOwnerGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.TransferOwnerGameObjectRoom;

        protected override bool Reliable => true;

        /// <summary>
        /// TransferOwnerGameObjectRoomOperationRequest
        /// </summary>
        /// <param name="objectId">The object id room game object</param>
        /// <param name="ownerId">The new owner id for this room game object</param>
        /// <param name="timeout"></param>
        public TransferOwnerGameObjectRoomOperationRequest(int objectId, int ownerId, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Add(ParameterCode.OwnerId, ownerId)
                .Build();
        }
    }
}
