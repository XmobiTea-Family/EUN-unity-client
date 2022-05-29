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
        public int PlayerId { get; private set; }

        /// <summary>
        /// The user id of this room player
        /// </summary>
        public string UserId { get; private set; }

        /// <summary>
        /// Custom player properties of this room player
        /// </summary>
        public EUNHashtable CustomProperties { get; private set; }

        public RoomPlayer(EUNArray eunArray)
        {
            PlayerId = eunArray.GetInt(0);
            UserId = eunArray.GetString(1);
            CustomProperties = eunArray.GetEUNHashtable(2);
        }
    }
}
