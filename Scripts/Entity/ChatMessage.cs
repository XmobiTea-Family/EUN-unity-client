namespace EUN.Entity
{
#if EUN
    using com.tvd12.ezyfoxserver.client.entity;
#endif
    public class ChatMessage
    {
        public string SenderId { get; private set; }
        public string Message { get; private set; }
        public long Ts { get; private set; }

#if EUN
        public ChatMessage(EzyArray ezyArray)
        {
            SenderId = ezyArray.get<string>(0);
            Message = ezyArray.get<string>(1);
            Ts = ezyArray.get<long>(2);
        }
#endif
    }
}