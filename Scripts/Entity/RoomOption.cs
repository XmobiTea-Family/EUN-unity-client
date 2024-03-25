namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;
    using System.Collections.Generic;

    /// <summary>
    /// The room option when create room
    /// </summary>
    public class RoomOption
    {
        /// <summary>
        /// The custom room properties
        /// </summary>
        public EUNHashtable customRoomProperties { get; private set; }

        /// <summary>
        /// The custom room property keys for lobby
        /// </summary>
        public List<int> customRoomPropertiesForLobby { get; private set; }

        /// <summary>
        /// The password room
        /// </summary>
        public string password { get; private set; }

        /// <summary>
        /// The max player for this room
        /// </summary>
        public int maxPlayer { get; private set; }

        /// <summary>
        /// The room isVisible for this room
        /// </summary>
        public bool isVisible { get; private set; }

        /// <summary>
        /// The room isOpen for this room
        /// </summary>
        public bool isOpen { get; private set; }

        /// <summary>
        /// Time to live mean time client can rejoin for this room after they disconnect
        /// When nobody in this room after they disconnect, and end time to live, the room will remove in lobby, and nobody can continue join this room
        /// </summary>
        public int ttl { get; private set; }

        private RoomOption(Builder builder)
        {
            this.customRoomProperties = builder.customRoomProperties;
            this.customRoomPropertiesForLobby = builder.customRoomPropertiesForLobby;
            this.password = builder.password;
            this.maxPlayer = builder.maxPlayer;
            this.isVisible = builder.isVisible;
            this.isOpen = builder.isOpen;
            this.ttl = builder.ttl;
        }

        private RoomOption() { }

        public class Builder
        {
            public EUNHashtable customRoomProperties;
            public List<int> customRoomPropertiesForLobby;
            public string password;
            public int maxPlayer = 4;
            public bool isVisible = true;
            public bool isOpen = true;
            public int ttl = 15000;

            /// <summary>
            /// The custom room properties
            /// </summary>
            /// <param name="customRoomProperties"></param>
            /// <returns></returns>
            public Builder setCustomRoomProperties(EUNHashtable customRoomProperties)
            {
                this.customRoomProperties = customRoomProperties;
                return this;
            }

            /// <summary>
            /// The custom room property keys for lobby
            /// </summary>
            /// <param name="customRoomPropertiesForLobby"></param>
            /// <returns></returns>
            public Builder setCustomRoomPropertiesForLobby(List<int> customRoomPropertiesForLobby)
            {
                this.customRoomPropertiesForLobby = customRoomPropertiesForLobby;
                return this;
            }

            /// <summary>
            /// The password room
            /// </summary>
            /// <param name="password"></param>
            /// <returns></returns>
            public Builder setPassword(string password)
            {
                this.password = password;
                return this;
            }

            /// <summary>
            /// The max player for this room
            /// </summary>
            /// <param name="maxPlayer"></param>
            /// <returns></returns>
            public Builder setMaxPlayer(int maxPlayer)
            {
                this.maxPlayer = maxPlayer;
                return this;
            }

            /// <summary>
            /// The max player for this room
            /// </summary>
            /// <param name="isVisible"></param>
            /// <returns></returns>
            public Builder setVisible(bool isVisible)
            {
                this.isVisible = isVisible;
                return this;
            }

            /// <summary>
            /// The room isOpen for this room
            /// </summary>
            /// <param name="isOpen"></param>
            /// <returns></returns>
            public Builder setOpen(bool isOpen)
            {
                this.isOpen = isOpen;
                return this;
            }

            /// <summary>
            /// Time to live mean time client can rejoin for this room after they disconnect
            /// When nobody in this room after they disconnect, and end time to live, the room will remove in lobby, and nobody can continue join this room
            /// </summary>
            /// <param name="ttl"></param>
            /// <returns></returns>
            public Builder setTtl(int ttl)
            {
                this.ttl = ttl;
                return this;
            }

            /// <summary>
            /// Build the RoomOption
            /// </summary>
            /// <returns></returns>
            public RoomOption build()
            {
                return new RoomOption(this);
            }
        }

    }

}
