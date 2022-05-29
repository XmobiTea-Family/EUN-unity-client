namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class SynchronizationDataGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.SynchronizationDataGameObjectRoom;

        protected override bool Reliable => false;

        /// <summary>
        /// SynchronizationDataGameObjectRoomOperationRequest
        /// </summary>
        /// <param name="objectId">The object id room game object</param>
        /// <param name="synchronizationData">The sync data</param>
        /// <param name="timeout"></param>
        public SynchronizationDataGameObjectRoomOperationRequest(int objectId, object synchronizationData, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Build();

            if (synchronizationData is object[] synchronizationDataObjects) Parameters.Add(ParameterCode.SynchronizationData, synchronizationDataObjects);
            else Parameters.Add(ParameterCode.SynchronizationData, synchronizationData);
        }
    }
}
