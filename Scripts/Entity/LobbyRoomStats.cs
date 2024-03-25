namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;

    public class LobbyRoomStats
    {
        /// <summary>
        /// The room id
        /// </summary>
        public int roomId { get; private set; }

        /// <summary>
        /// The room is open
        /// </summary>
        public bool isOpen { get; private set; }

        /// <summary>
        /// The max player of room
        /// </summary>
        public int maxPlayer { get; private set; }

        /// <summary>
        /// The room has contains password
        /// </summary>
        public bool hasPassword { get; private set; }

        /// <summary>
        /// The current player count in room
        /// </summary>
        public int playerCount { get; private set; }

        /// <summary>
        /// The custom room properties can get in lobby
        /// </summary>
        public EUNHashtable customRoomPropertiesForLobby { get; private set; }

        public LobbyRoomStats(EUNArray eunArray)
        {
            this.roomId = eunArray.getInt(0);
            this.isOpen = eunArray.getBool(1);
            this.maxPlayer = eunArray.getInt(2);
            this.hasPassword = eunArray.getBool(3);
            this.playerCount = eunArray.getInt(4);
            this.customRoomPropertiesForLobby = eunArray.getEUNHashtable(5);
        }

    }

}
