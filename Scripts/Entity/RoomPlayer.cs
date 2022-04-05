namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;

    public class RoomPlayer
    {
        public int PlayerId { get; private set; }
        public string UserId { get; private set; }
        public EUNHashtable CustomProperties { get; private set; }

        public RoomPlayer(EUNArray eunArray)
        {
            PlayerId = eunArray.GetInt(0);
            UserId = eunArray.GetString(1);
            CustomProperties = eunArray.GetEUNHashtable(2);
        }
    }
}