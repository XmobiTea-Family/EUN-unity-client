namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;

    using System.Collections.Generic;

    public class Room
    {
        public int RoomId { get; set; }
        public bool IsOpen { get; set; }
        public int MaxPlayer { get; set; }
        public string Password { get; set; }
        public List<RoomPlayer> RoomPlayerLst { get; set; }
        public EUNHashtable CustomRoomProperties { get; set; }
        public List<int> CustomRoomPropertiesForLobby { get; set; }
        public bool IsVisible { get; set; }
        public string LeaderClientUserId { get; set; }
        public long TsCreate { get; set; }
        public int Ttl { get; set; }
        public Dictionary<int, RoomGameObject> GameObjectDic { get; private set; }

        public RoomPlayer[] GetRoomPlayers()
        {
            return new List<RoomPlayer>(RoomPlayerLst).ToArray();
        }

        public Room(EUNArray eunArray)
        {
            this.RoomPlayerLst = new List<RoomPlayer>();
            {
                var eunArray1 = eunArray.GetEUNArray(4);
                for (var i = 0; i < eunArray1.Count(); i++)
                {
                    RoomPlayerLst.Add(new RoomPlayer(eunArray1.GetEUNArray(i)));
                }
            }

            this.CustomRoomPropertiesForLobby = new List<int>();
            {
                var eunArray1 = eunArray.GetEUNArray(6);
                for (var i = 0; i < eunArray1.Count(); i++)
                {
                    CustomRoomPropertiesForLobby.Add(eunArray1.GetInt(i));
                }
            }

            this.GameObjectDic = new Dictionary<int, RoomGameObject>();
            {
                var eunArray1 = eunArray.GetEUNArray(11);
                for (var i = 0; i < eunArray1.Count(); i++)
                {
                    var roomGameObject = new RoomGameObject(eunArray1.GetEUNArray(i));

                    this.GameObjectDic[roomGameObject.ObjectId] = roomGameObject;
                }
            }

            this.RoomId = eunArray.GetInt(0);
            this.IsOpen = eunArray.GetBool(1);
            this.MaxPlayer = eunArray.GetInt(2);
            this.Password = eunArray.GetString(3);

            this.CustomRoomProperties = eunArray.GetEUNHashtable(5);

            this.IsVisible = eunArray.GetBool(7);
            this.LeaderClientUserId = eunArray.GetString(8);
            this.TsCreate = eunArray.GetLong(9);
            this.Ttl = eunArray.GetInt(10);
        }
    }
}