namespace EUN.Entity
{
#if EUN
    using com.tvd12.ezyfoxserver.client.entity;
#endif
    using EUN.Common;

    public class LobbyRoomStats
    {
        public int RoomId { get; private set; }
        public bool IsOpen { get; private set; }

        public int MaxPlayer { get; private set; }
        public bool HasPassword { get; private set; }
        public int PlayerCount { get; private set; }
        public CustomHashtable CustomRoomPropertiesForLobby { get; private set; }

#if EUN
        public LobbyRoomStats(EzyArray ezyArray)
        {
            RoomId = ezyArray.get<int>(0);
            IsOpen = ezyArray.get<bool>(1);
            MaxPlayer = ezyArray.get<int>(2);
            HasPassword = ezyArray.get<bool>(3);
            PlayerCount = ezyArray.get<int>(4);
            CustomRoomPropertiesForLobby = new CustomHashtable(ezyArray.get<EzyObject>(5));
        }
#endif
    }
}