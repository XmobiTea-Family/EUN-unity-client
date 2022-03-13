namespace EUN.Entity
{
    using EUN.Common;

    using System.Collections.Generic;

    public class Room
    {
        public int RoomId { get; set; }
        public bool IsOpen { get; set; }
        public int MaxPlayer { get; set; }
        public string Password { get; set; }
        public List<RoomPlayer> RoomPlayerLst { get; set; }
        public CustomHashtable CustomRoomProperties { get; set; }
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

        public Room(CustomArray customArray)
        {
            this.RoomPlayerLst = new List<RoomPlayer>();
            {
                var ezyArray1 = customArray.GetCustomArray(4);
                for (var i = 0; i < ezyArray1.Count(); i++)
                {
                    RoomPlayerLst.Add(new RoomPlayer(ezyArray1.GetCustomArray(i)));
                }
            }

            this.CustomRoomPropertiesForLobby = new List<int>();
            {
                var ezyArray1 = customArray.GetCustomArray(6);
                for (var i = 0; i < ezyArray1.Count(); i++)
                {
                    CustomRoomPropertiesForLobby.Add(ezyArray1.GetInt(i));
                }
            }

            this.GameObjectDic = new Dictionary<int, RoomGameObject>();
            {
                var ezyArray1 = customArray.GetCustomArray(11);
                for (var i = 0; i < ezyArray1.Count(); i++)
                {
                    var roomGameObject = new RoomGameObject(ezyArray1.GetCustomArray(i));

                    this.GameObjectDic[roomGameObject.ObjectId] = roomGameObject;
                }
            }

            this.RoomId = customArray.GetInt(0);
            this.IsOpen = customArray.GetBool(1);
            this.MaxPlayer = customArray.GetInt(2);
            this.Password = customArray.GetString(3);

            this.CustomRoomProperties = customArray.GetCustomHashtable(5);

            this.IsVisible = customArray.GetBool(7);
            this.LeaderClientUserId = customArray.GetString(8);
            this.TsCreate = customArray.GetLong(9);
            this.Ttl = customArray.GetInt(10);
        }
    }
}