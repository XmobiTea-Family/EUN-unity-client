namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class SynchronizationDataGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.SynchronizationDataGameObjectRoom;

        protected override bool Reliable => false;

        public SynchronizationDataGameObjectRoomOperationRequest(int objectId, object synchronizationData, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Build();

            if (synchronizationData is object[] synchronizationDataObjects) Parameters.Add(ParameterCode.SynchronizationData, synchronizationDataObjects);
            else Parameters.Add(ParameterCode.SynchronizationData, synchronizationData);
        }
    }
}