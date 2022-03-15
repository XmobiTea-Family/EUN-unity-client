namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class DestroyGameObjectRoomOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.DestroyGameObjectRoom;

        protected override bool Reliable => true;

        public DestroyGameObjectRoomOperationRequest(int objectId, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Build();
        }
    }
}
