namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class SynchronizationDataGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.SynchronizationDataGameObjectRoom;

        protected override bool reliable => false;

        /// <summary>
        /// SynchronizationDataGameObjectRoomOperationRequest
        /// </summary>
        /// <param name="objectId">The object id room game object</param>
        /// <param name="synchronizationData">The sync data</param>
        /// <param name="timeout"></param>
        public SynchronizationDataGameObjectRoomOperationRequest(int objectId, object synchronizationData, int timeout = OperationRequest.defaultTimeout) : base(timeout)
        {
            this.parameters = new EUNHashtable.Builder()
                .add(ParameterCode.ObjectId, objectId)
                .build();

            if (synchronizationData is object[] synchronizationDataObjects) parameters.add(ParameterCode.SynchronizationData, synchronizationDataObjects);
            else parameters.add(ParameterCode.SynchronizationData, synchronizationData);
        }

    }

}
