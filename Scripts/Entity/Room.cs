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
        public int roomId { get; private set; }

        /// <summary>
        /// The room is open or not
        /// if isOpen = false, other client cant join this room
        /// </summary>
        public bool isOpen { get; internal set; }

        /// <summary>
        /// The max player in this room
        /// If current player count in this room less than max player, other client can join this room
        /// </summary>
        public int maxPlayer { get; internal set; }

        /// <summary>
        /// The password in this room
        /// If the password isNullOrEmpty it mean this room no has password
        /// Other client need input right password to join this room
        /// </summary>
        public string password { get; internal set; }

        /// <summary>
        /// Current room player list in this room
        /// </summary>
        public List<RoomPlayer> roomPlayerLst { get; private set; }

        /// <summary>
        /// Custom room properties in this room
        /// </summary>
        public EUNHashtable customRoomProperties { get; private set; }

        /// <summary>
        /// Custom room properties key in this room for all player does not inroom can see 
        /// </summary>
        public List<int> customRoomPropertiesForLobby { get; private set; }

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
        public Dictionary<int, RoomGameObject> gameObjectDict { get; private set; }

        /// <summary>
        /// The the current room player array in this room
        /// </summary>
        /// <returns></returns>
        public List<RoomPlayer> getRoomPlayerLst()
        {
            return roomPlayerLst;
        }

        public Room(EUNArray eunArray)
        {
            this.roomPlayerLst = new List<RoomPlayer>();
            {
                var eunArray1 = eunArray.getEUNArray(4);
                for (var i = 0; i < eunArray1.count(); i++)
                {
                    this.roomPlayerLst.Add(new RoomPlayer(eunArray1.getEUNArray(i)));
                }
            }

            this.customRoomPropertiesForLobby = new List<int>();
            {
                var eunArray1 = eunArray.getEUNArray(6);
                for (var i = 0; i < eunArray1.count(); i++)
                {
                    this.customRoomPropertiesForLobby.Add(eunArray1.getInt(i));
                }
            }

            this.gameObjectDict = new Dictionary<int, RoomGameObject>();
            {
                var eunArray1 = eunArray.getEUNArray(11);
                for (var i = 0; i < eunArray1.count(); i++)
                {
                    var roomGameObject = new RoomGameObject(eunArray1.getEUNArray(i));

                    this.gameObjectDict[roomGameObject.objectId] = roomGameObject;
                }
            }

            this.roomId = eunArray.getInt(0);
            this.isOpen = eunArray.getBool(1);
            this.maxPlayer = eunArray.getInt(2);
            this.password = eunArray.getString(3);

            this.customRoomProperties = eunArray.getEUNHashtable(5);

            this.isVisible = eunArray.getBool(7);
            this.leaderClientUserId = eunArray.getString(8);
            this.tsCreate = eunArray.getLong(9);
            this.ttl = eunArray.getInt(10);
        }

    }

}
