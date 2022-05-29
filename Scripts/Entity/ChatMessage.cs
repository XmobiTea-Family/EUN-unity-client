namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;

    /// <summary>
    /// The chat message receive if somebody send chat
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// The user id of sender
        /// </summary>
        public string SenderId { get; private set; }

        /// <summary>
        /// The message content
        /// </summary>
        public string Message { get; private set; }

        public ChatMessage(EUNArray eunArray)
        {
            SenderId = eunArray.GetString(0);
            Message = eunArray.GetString(1);
        }
    }
}
