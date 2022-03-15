namespace EUN.Entity.Request
{
    using EUN.Common;
    using EUN.Constant;

    public class VoiceChatOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.VoiceChat;

        protected override bool Reliable => false;

        public VoiceChatOperationRequest(int objectId, object voiceChatData, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new CustomHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Add(ParameterCode.SynchronizationData, voiceChatData)
                .Build();
        }
    }
}
