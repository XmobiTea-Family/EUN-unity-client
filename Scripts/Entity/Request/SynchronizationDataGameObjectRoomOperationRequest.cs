namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class SynchronizationDataGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.SynchronizationDataGameObjectRoom;

        protected override bool Reliable => false;

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
