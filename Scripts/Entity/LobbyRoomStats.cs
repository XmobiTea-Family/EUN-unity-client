namespace EUN.Entity
{
    using com.tvd12.ezyfoxserver.client.entity;

    using EUN.Common;

    public class LobbyRoomStats
    {
        public int RoomId { get; private set; }
        public bool IsOpen { get; private set; }

        public int MaxPlayer { get; private set; }
        public bool HasPassword { get; private set; }
        public int PlayerCount { get; private set; }
        public CustomHashtable CustomRoomPropertiesForLobby { get; private set; }

        public LobbyRoomStats(EzyArray ezyArray)
        {
            RoomId = ezyArray.get<int>(0);
            IsOpen = ezyArray.get<bool>(1);
            MaxPlayer = ezyArray.get<int>(2);
            HasPassword = ezyArray.get<bool>(3);
            PlayerCount = ezyArray.get<int>(4);
            CustomRoomPropertiesForLobby = new CustomHashtable(ezyArray.get<EzyObject>(5));
        }
    }
}