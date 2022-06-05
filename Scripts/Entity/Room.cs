namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;

    using System.Collections.Generic;

    /// <summary>
    /// The full room infomation
    /// </summary>
    public class Room : LobbyRoomStats
    {
        /// <summary>
        /// The password in this room
        /// If the password isNullOrEmpty it mean this room no has password
        /// Other client need input right password to join this room
        /// </summary>
        public string password { get; internal set; }

        public override bool hasPassword { get => !string.IsNullOrEmpty(password); protected set => base.hasPassword = value; }

        /// <summary>
        /// Current room player list in this room
        /// </summary>
        public List<RoomPlayer> roomPlayerLst { get; private set; }

        public override int playerCount { get => roomPlayerLst.Count; protected set => base.playerCount = value; }

        /// <summary>
        /// Custom room properties in this room
        /// </summary>
        public EUNHashtable customRoomProperties { get; private set; }

        /// <summary>
        /// Custom room properties key in this room for all player does not inroom can see 
        /// </summary>
        public List<int> customRoomPropertiesForLobbyLst { get; private set; }

        /// <summary>
        /// The room is visible or not
        /// If is visible = false, other client cant find this room in global
        /// </summary>
        public bool isVisible { get; internal set; }

        /// <summary>
        /// The leaderclient user id in this room
        /// </summary>
        public string leaderClientUserId { get; internal set; }

        /// <summary>
        /// The create room time in milliseconds
        /// </summary>
        public long tsCreate { get; private set; }

        /// <summary>
        /// Time to live in this room
        /// </summary>
        public int ttl { get; internal set; }

        /// <summary>
        /// The room game object dict, with key is object id and value is room game object
        /// </summary>
        public Dictionary<int, RoomGameObject> gameObjectDic { get; private set; }

        /// <summary>
        /// The the current room player array in this room
        /// </summary>
        /// <returns></returns>
        public RoomPlayer[] GetRoomPlayers()
        {
            return roomPlayerLst.ToArray();
        }

        public Room(EUNArray eunArray) : base()
        {
            this.roomPlayerLst = new List<RoomPlayer>();
            {
                var eunArray1 = eunArray.GetEUNArray(4);
                for (var i = 0; i < eunArray1.Count(); i++)
                {
                    roomPlayerLst.Add(new RoomPlayer(eunArray1.GetEUNArray(i)));
                }
            }

            this.customRoomPropertiesForLobbyLst = new List<int>();
            {
                var eunArray1 = eunArray.GetEUNArray(6);
                for (var i = 0; i < eunArray1.Count(); i++)
                {
                    customRoomPropertiesForLobbyLst.Add(eunArray1.GetInt(i));
                }
            }

            this.gameObjectDic = new Dictionary<int, RoomGameObject>();
            {
                var eunArray1 = eunArray.GetEUNArray(11);
                for (var i = 0; i < eunArray1.Count(); i++)
                {
                    var roomGameObject = new RoomGameObject(eunArray1.GetEUNArray(i));

                    this.gameObjectDic[roomGameObject.objectId] = roomGameObject;
                }
            }

            this.roomId = eunArray.GetInt(0);
            this.isOpen = eunArray.GetBool(1);
            this.maxPlayer = eunArray.GetInt(2);
            this.password = eunArray.GetString(3);

            this.customRoomProperties = eunArray.GetEUNHashtable(5);

            this.isVisible = eunArray.GetBool(7);
            this.leaderClientUserId = eunArray.GetString(8);
            this.tsCreate = eunArray.GetLong(9);
            this.ttl = eunArray.GetInt(10);
        }
    }
}
