namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;

    public class LobbyRoomStats
    {
        /// <summary>
        /// The room id
        /// </summary>
        public int RoomId { get; private set; }

        /// <summary>
        /// The room is open
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// The max player of room
        /// </summary>
        public int MaxPlayer { get; private set; }

        /// <summary>
        /// The room has contains password
        /// </summary>
        public bool HasPassword { get; private set; }

        /// <summary>
        /// The current player count in room
        /// </summary>
        public int PlayerCount { get; private set; }

        /// <summary>
        /// The custom room properties can get in lobby
        /// </summary>
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
