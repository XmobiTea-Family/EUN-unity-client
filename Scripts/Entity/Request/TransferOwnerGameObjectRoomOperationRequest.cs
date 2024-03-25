namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class TransferOwnerGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.TransferOwnerGameObjectRoom;

        protected override bool reliable => true;

        /// <summary>
        /// TransferOwnerGameObjectRoomOperationRequest
        /// </summary>
        /// <param name="objectId">The object id room game object</param>
        /// <param name="ownerId">The new owner id for this room game object</param>
        /// <param name="timeout"></param>
        public TransferOwnerGameObjectRoomOperationRequest(int objectId, int ownerId, int timeout = OperationRequest.defaultTimeout) : base(timeout)
        {
            this.parameters = new EUNHashtable.Builder()
                .add(ParameterCode.ObjectId, objectId)
                .add(ParameterCode.OwnerId, ownerId)
                .build();
        }

    }

}
