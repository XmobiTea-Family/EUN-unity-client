namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class DestroyGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.DestroyGameObjectRoom;

        protected override bool Reliable => true;

        /// <summary>
        /// DestroyGameObjectRoomOperationRequest
        /// </summary>
        /// <param name="objectId">The object id room game object need destroy</param>
        /// <param name="timeout"></param>
        public DestroyGameObjectRoomOperationRequest(int objectId, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Build();
        }
    }
}
