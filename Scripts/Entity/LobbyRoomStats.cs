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
        public CustomHashtable CustomRoomPropertiesForLobby { get; private set; }

        public LobbyRoomStats(CustomArray customArray)
        {
            RoomId = customArray.GetInt(0);
            IsOpen = customArray.GetBool(1);
            MaxPlayer = customArray.GetInt(2);
            HasPassword = customArray.GetBool(3);
            PlayerCount = customArray.GetInt(4);
            CustomRoomPropertiesForLobby = customArray.GetCustomHashtable(5);
        }
    }
}