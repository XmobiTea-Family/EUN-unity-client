namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class CreateGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.CreateGameObjectRoom;

        protected override bool Reliable => true;

        public CreateGameObjectRoomOperationRequest(string prefabPath, object initializeData, object synchronizationData, EUNHashtable customGameObjectProperties, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.PrefabPath, prefabPath)
                .Add(ParameterCode.InitializeData, initializeData)
                .Add(ParameterCode.SynchronizationData, synchronizationData)
                .Add(ParameterCode.CustomGameObjectProperties, customGameObjectProperties)
                .Build();
        }
    }
}
