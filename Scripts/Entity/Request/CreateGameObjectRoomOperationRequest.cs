namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class CreateGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.CreateGameObjectRoom;

        protected override bool reliable => true;

        /// <summary>
        /// CreateGameObjectRoomOperationRequest
        /// </summary>
        /// <param name="prefabPath">The prefab path of game object</param>
        /// <param name="initializeData">The initialize data of game object</param>
        /// <param name="synchronizationData">The sync data of game object</param>
        /// <param name="customGameObjectProperties">The custom game object properties</param>
        /// <param name="timeout"></param>
        public CreateGameObjectRoomOperationRequest(string prefabPath, object initializeData, object synchronizationData, EUNHashtable customGameObjectProperties, int timeout = OperationRequest.defaultTimeout) : base(timeout)
        {
            this.parameters = new EUNHashtable.Builder()
                .add(ParameterCode.PrefabPath, prefabPath)
                .add(ParameterCode.InitializeData, initializeData)
                .add(ParameterCode.SynchronizationData, synchronizationData)
                .add(ParameterCode.CustomGameObjectProperties, customGameObjectProperties)
                .build();
        }

    }

}
