namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class VoiceChatOperationRequest : CustomOperationRequest
    {
        protected override int Code => OperationCode.VoiceChat;

        protected override bool Reliable => false;

        /// <summary>
        /// VoiceChatOperationRequest
        /// </summary>
        /// <param name="objectId">The object id room game object</param>
        /// <param name="voiceChatData">The voice chat data for room game object</param>
        /// <param name="timeout"></param>
        public VoiceChatOperationRequest(int objectId, object voiceChatData, int timeout = OperationRequest.DefaultTimeOut) : base(timeout)
        {
            Parameters = new EUNHashtable.Builder()
                .Add(ParameterCode.ObjectId, objectId)
                .Add(ParameterCode.SynchronizationData, voiceChatData)
                .Build();
        }
    }
}
