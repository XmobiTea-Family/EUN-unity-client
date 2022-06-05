namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;

    public class LobbyStats
    {
        /// <summary>
        /// The lobby id
        /// </summary>
        public int lobbyId { get; private set; }

        /// <summary>
        /// The player count in this lobby
        /// </summary>
        public int playerCount { get; private set; }

        /// <summary>
        /// The room count in this lobby
        /// </summary>
        public int roomCount { get; private set; }

        public LobbyStats(EUNArray eunArray)
        {
            lobbyId = eunArray.GetInt(0);
            playerCount = eunArray.GetInt(1);
            roomCount = eunArray.GetInt(2);
        }
    }
}
