namespace EUN.Entity
{
    using EUN.Common;

    public class ChatMessage
    {
        public string SenderId { get; private set; }
        public string Message { get; private set; }
        public long Ts { get; private set; }

        public ChatMessage(CustomArray customArray)
        {
            SenderId = customArray.GetString(0);
            Message = customArray.GetString(1);
            Ts = customArray.GetLong(2);
        }
    }
}