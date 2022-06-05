namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;

    public class LobbyRoomStats
    {
        /// <summary>
        /// The room id
        /// </summary>
        public int roomId { get; protected set; }

        /// <summary>
        /// The room is open or not
        /// if isOpen = false, other client cant join this room
        /// </summary>
        public bool isOpen { get; internal set; }

        /// <summary>
        /// The max player in this room
        /// If current player count in this room less than max player, other client can join this room
        /// </summary>
        public int maxPlayer { get; internal set; }

        /// <summary>
        /// The room has contains password
        /// </summary>
        public virtual bool hasPassword { get; protected set; }

        /// <summary>
        /// The current player count in room
        /// </summary>
        public virtual int playerCount { get; protected set; }

        /// <summary>
        /// Custom room properties in this room for all player does not inroom can see
        /// And all player inroom can not see this info
        /// </summary>
        public EUNHashtable customRoomPropertiesForLobby { get; protected set; }

        internal LobbyRoomStats(EUNArray eunArray)
        {
            roomId = eunArray.GetInt(0);
            isOpen = eunArray.GetBool(1);
            maxPlayer = eunArray.GetInt(2);
            hasPassword = eunArray.GetBool(3);
            playerCount = eunArray.GetInt(4);
            customRoomPropertiesForLobby = eunArray.GetEUNHashtable(5);
        }

        internal LobbyRoomStats()
        {

        }
    }
}
