namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;

    public class RoomPlayer
    {
        public int PlayerId { get; private set; }
        public string UserId { get; private set; }
        public CustomHashtable CustomProperties { get; private set; }

        public RoomPlayer(CustomArray customArray)
        {
            PlayerId = customArray.GetInt(0);
            UserId = customArray.GetString(1);
            CustomProperties = customArray.GetCustomHashtable(2);
        }
    }
}