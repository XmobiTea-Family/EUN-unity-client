namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;

    public class LobbyRoomStats
    {
        public int RoomId { get; private set; }
        public bool IsOpen { get; private set; }

        public int MaxPlayer { get; private set; }
        public bool HasPassword { get; private set; }
        public int PlayerCount { get; private set; }
        public EUNHashtable CustomRoomPropertiesForLobby { get; private set; }

        public LobbyRoomStats(EUNArray eunArray)
        {
            RoomId = eunArray.GetInt(0);
            IsOpen = eunArray.GetBool(1);
            MaxPlayer = eunArray.GetInt(2);
            HasPassword = eunArray.GetBool(3);
            PlayerCount = eunArray.GetInt(4);
            CustomRoomPropertiesForLobby = eunArray.GetEUNHashtable(5);
        }
    }
}