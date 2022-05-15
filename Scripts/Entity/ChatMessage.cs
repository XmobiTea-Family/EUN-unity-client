namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;

    public class ChatMessage
    {
        public string SenderId { get; private set; }
        public string Message { get; private set; }
        public long Ts { get; private set; }

        public ChatMessage(EUNArray eunArray)
        {
            SenderId = eunArray.GetString(0);
            Message = eunArray.GetString(1);
            Ts = eunArray.GetLong(2);
        }
    }
}
