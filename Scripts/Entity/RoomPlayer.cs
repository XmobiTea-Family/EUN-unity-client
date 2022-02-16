namespace EUN.Entity
{
    using com.tvd12.ezyfoxserver.client.entity;

    using EUN.Common;

    public class RoomPlayer
    {
        public int PlayerId { get; private set; }
        public string UserId { get; private set; }
        public CustomHashtable CustomProperties { get; private set; }

        public RoomPlayer(EzyArray ezyArray)
        {
            PlayerId = ezyArray.get<int>(0);
            UserId = ezyArray.get<string>(1);
            CustomProperties = new CustomHashtable(ezyArray.get<EzyObject>(2));
        }
    }
}