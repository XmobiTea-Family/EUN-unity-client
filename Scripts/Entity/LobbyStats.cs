namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;

    public class LobbyStats
    {
        public int LobbyId { get; private set; }
        public int PlayerSize { get; private set; }
        public int RoomSize { get; private set; }

        public LobbyStats(CustomArray customArray)
        {
            LobbyId = customArray.GetInt(0);
            PlayerSize = customArray.GetInt(1);
            RoomSize = customArray.GetInt(2);
        }
    }
}