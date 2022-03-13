namespace EUN.Entity
{
#if EUN
    using com.tvd12.ezyfoxserver.client.entity;
#endif
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

#if EUN
        public Room(EzyArray ezyArray)
        {
            this.RoomPlayerLst = new List<RoomPlayer>();
            {
                var ezyArray1 = ezyArray.get<EzyArray>(4);
                for (var i = 0; i < ezyArray1.size(); i++)
                {
                    RoomPlayerLst.Add(new RoomPlayer(ezyArray1.get<EzyArray>(i)));
                }
            }

            this.CustomRoomPropertiesForLobby = new List<int>();
            {
                var ezyArray1 = ezyArray.get<EzyArray>(6);
                for (var i = 0; i < ezyArray1.size(); i++)
                {
                    CustomRoomPropertiesForLobby.Add(ezyArray1.get<int>(i));
                }
            }

            this.GameObjectDic = new Dictionary<int, RoomGameObject>();
            {
                var ezyArray1 = ezyArray.get<EzyArray>(11);
                for (var i = 0; i < ezyArray1.size(); i++)
                {
                    var roomGameObject = new RoomGameObject(ezyArray1.get<EzyArray>(i));

                    this.GameObjectDic[roomGameObject.ObjectId] = roomGameObject;
                }
            }

            this.RoomId = ezyArray.get<int>(0);
            this.IsOpen = ezyArray.get<bool>(1);
            this.MaxPlayer = ezyArray.get<int>(2);
            this.Password = ezyArray.get<string>(3);

            this.CustomRoomProperties = new CustomHashtable(ezyArray.get<EzyObject>(5));

            this.IsVisible = ezyArray.get<bool>(7);
            this.LeaderClientUserId = ezyArray.get<string>(8);
            this.TsCreate = ezyArray.get<long>(9);
            this.Ttl = ezyArray.get<int>(10);
        }
#endif
    }
}