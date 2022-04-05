namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class VoiceChatOperationRequest : CustomOperationRequest
    {
        protected override OperationCode Code => OperationCode.VoiceChat;

        protected override bool Reliable => false;

        public VoiceChatOperationRequest(int objectId, object voiceChatData, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Add(ParameterCode.SynchronizationData, voiceChatData)
                .Build();
        }
    }
}
