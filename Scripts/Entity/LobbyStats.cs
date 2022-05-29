namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;

    public class LobbyStats
    {
        /// <summary>
        /// The lobby id
        /// </summary>
        public int LobbyId { get; private set; }

        /// <summary>
        /// The player count in this lobby
        /// </summary>
        public int PlayerCount { get; private set; }

        /// <summary>
        /// The room count in this lobby
        /// </summary>
        public int RoomCount { get; private set; }

        public LobbyStats(EUNArray eunArray)
        {
            LobbyId = eunArray.GetInt(0);
            PlayerCount = eunArray.GetInt(1);
            RoomCount = eunArray.GetInt(2);
        }
    }
}
