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
        public SynchronizationDataGameObjectRoomOperationRequest(int objectId, object synchronizationData, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Build();

            if (synchronizationData is object[] synchronizationDataObjects) parameters.Add(ParameterCode.SynchronizationData, synchronizationDataObjects);
            else parameters.Add(ParameterCode.SynchronizationData, synchronizationData);
        }
    }
}
