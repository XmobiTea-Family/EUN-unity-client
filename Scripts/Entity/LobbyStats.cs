namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;

    public class LobbyStats
    {
        public int LobbyId { get; private set; }
        public int PlayerSize { get; private set; }
        public int RoomSize { get; private set; }

        public LobbyStats(EUNArray eunArray)
        {
            LobbyId = eunArray.GetInt(0);
            PlayerSize = eunArray.GetInt(1);
            RoomSize = eunArray.GetInt(2);
        }
    }
}
