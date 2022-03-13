namespace EUN.Entity
{
#if EUN
    using com.tvd12.ezyfoxserver.client.entity;
#endif
    using EUN.Common;

    public class RoomPlayer
    {
        public int PlayerId { get; private set; }
        public string UserId { get; private set; }
        public CustomHashtable CustomProperties { get; private set; }

#if EUN
        public RoomPlayer(EzyArray ezyArray)
        {
            PlayerId = ezyArray.get<int>(0);
            UserId = ezyArray.get<string>(1);
            CustomProperties = new CustomHashtable(ezyArray.get<EzyObject>(2));
        }
#endif
    }
}