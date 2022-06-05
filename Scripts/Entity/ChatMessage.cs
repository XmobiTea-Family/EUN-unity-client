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
        public string senderId { get; private set; }

        /// <summary>
        /// The message content
        /// </summary>
        public string message { get; private set; }

        public ChatMessage(EUNArray eunArray)
        {
            senderId = eunArray.GetString(0);
            message = eunArray.GetString(1);
        }
    }
}
