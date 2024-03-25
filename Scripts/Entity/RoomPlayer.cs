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
            this.playerId = eunArray.getInt(0);
            this.userId = eunArray.getString(1);
            this.customProperties = eunArray.getEUNHashtable(2);
        }

    }

}
