namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;

    /// <summary>
    /// The room player in this room
    /// </summary>
    public class RoomPlayer
    {
        /// <summary>
        /// The player id of this room player
        /// </summary>
        public int playerId { get; private set; }

        /// <summary>
        /// The user id of this room player
        /// </summary>
        public string userId { get; private set; }

        /// <summary>
        /// Custom player properties of this room player
        /// </summary>
        public EUNHashtable customProperties { get; private set; }

        public RoomPlayer(EUNArray eunArray)
        {
            playerId = eunArray.GetInt(0);
            userId = eunArray.GetString(1);
            customProperties = eunArray.GetEUNHashtable(2);
        }
    }
}
