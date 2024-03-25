namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public class VoiceChatOperationRequest : CustomOperationRequest
    {
        protected override int code => OperationCode.VoiceChat;

        protected override bool reliable => false;

        /// <summary>
        /// VoiceChatOperationRequest
        /// </summary>
        /// <param name="objectId">The object id room game object</param>
        /// <param name="voiceChatData">The voice chat data for room game object</param>
        /// <param name="timeout"></param>
        public VoiceChatOperationRequest(int objectId, object voiceChatData, int timeout = OperationRequest.defaultTimeout) : base(timeout)
        {
            this.parameters = new EUNHashtable.Builder()
                .add(ParameterCode.ObjectId, objectId)
                .add(ParameterCode.SynchronizationData, voiceChatData)
                .build();
        }

    }

}
