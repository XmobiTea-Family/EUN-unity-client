namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;

    using System.Collections.Generic;

    /// <summary>
    /// The full room infomation
    /// </summary>
    public class Room
    {
        /// <summary>
        /// The room id
        /// </summary>
        public int RoomId { get; private set; }

        /// <summary>
        /// The room is open or not
        /// if isOpen = false, other client cant join this room
        /// </summary>
        public bool IsOpen { get; internal set; }

        /// <summary>
        /// The max player in this room
        /// If current player count in this room less than max player, other client can join this room
        /// </summary>
        public int MaxPlayer { get; internal set; }

        /// <summary>
        /// The password in this room
        /// If the password isNullOrEmpty it mean this room no has password
        /// Other client need input right password to join this room
        /// </summary>
        public string Password { get; internal set; }

        /// <summary>
        /// Current room player list in this room
        /// </summary>
        public List<RoomPlayer> RoomPlayerLst { get; private set; }

        /// <summary>
        /// Custom room properties in this room
        /// </summary>
        public EUNHashtable CustomRoomProperties { get; private set; }

        /// <summary>
        /// Custom room properties key in this room for all player does not inroom can see 
        /// </summary>
        public List<int> CustomRoomPropertiesForLobby { get; private set; }

        /// <summary>
        /// The room is visible or not
        /// If is visible = false, other client cant find this room in global
        /// </summary>
        public bool IsVisible { get; internal set; }

        /// <summary>
        /// The leaderclient user id in this room
        /// </summary>
        public string LeaderClientUserId { get; internal set; }

        /// <summary>
        /// The create room time in milliseconds
        /// </summary>
        public long TsCreate { get; private set; }

        /// <summary>
        /// Time to live in this room
        /// </summary>
        public int Ttl { get; internal set; }

        /// <summary>
        /// The room game object dict, with key is object id and value is room game object
        /// </summary>
        public Dictionary<int, RoomGameObject> GameObjectDic { get; private set; }

        /// <summary>
        /// The the current room player array in this room
        /// </summary>
        /// <returns></returns>
        public RoomPlayer[] GetRoomPlayers()
        {
            return RoomPlayerLst.ToArray();
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
