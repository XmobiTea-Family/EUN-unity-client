namespace EUN.Entity
{
#if EUN
    using com.tvd12.ezyfoxserver.client.entity;
#endif
    public class LobbyStats
    {
        public int LobbyId { get; private set; }
        public int PlayerSize { get; private set; }
        public int RoomSize { get; private set; }

#if EUN
        public LobbyStats(EzyArray ezyArray)
        {
            LobbyId = ezyArray.get<int>(0);
            PlayerSize = ezyArray.get<int>(1);
            RoomSize = ezyArray.get<int>(2);
        }
#endif
    }
}