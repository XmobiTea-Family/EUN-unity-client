namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class DestroyGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.DestroyGameObjectRoom;

        protected override bool reliable => true;

        /// <summary>
        /// DestroyGameObjectRoomOperationRequest
        /// </summary>
        /// <param name="objectId">The object id room game object need destroy</param>
        /// <param name="timeout"></param>
        public DestroyGameObjectRoomOperationRequest(int objectId, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Build();
        }
    }
}
