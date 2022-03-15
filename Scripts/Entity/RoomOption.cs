namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;
    using System.Collections.Generic;

    public class RoomOption
    {
        public CustomHashtable CustomRoomProperties { get; private set; }
        public List<int> CustomRoomPropertiesForLobby { get; private set; }
        public string Password { get; private set; }
        public int MaxPlayer { get; private set; }
        public bool IsVisible { get; private set; }
        public bool IsOpen { get; private set; }
        public int Ttl { get; private set; }

        public RoomOption(Builder builder)
        {
            this.CustomRoomProperties = builder.customRoomProperties;
            this.CustomRoomPropertiesForLobby = builder.customRoomPropertiesForLobby;
            this.Password = builder.password;
            this.MaxPlayer = builder.maxPlayer;
            this.IsVisible = builder.isVisible;
            this.IsOpen = builder.isOpen;
            this.Ttl = builder.ttl;
        }

        public class Builder
        {
            public CustomHashtable customRoomProperties;
            public List<int> customRoomPropertiesForLobby;
            public string password;
            public int maxPlayer = 4;
            public bool isVisible = true;
            public bool isOpen = true;
            public int ttl = 15000;

            public Builder SetCustomRoomProperties(CustomHashtable customRoomProperties)
            {
                this.customRoomProperties = customRoomProperties;
                return this;
            }

            public Builder SetCustomRoomPropertiesForLobby(List<int> customRoomPropertiesForLobby)
            {
                this.customRoomPropertiesForLobby = customRoomPropertiesForLobby;
                return this;
            }

            public Builder SetPassword(string password)
            {
                this.password = password;
                return this;
            }


            public Builder SetMaxPlayer(int maxPlayer)
            {
                this.maxPlayer = maxPlayer;
                return this;
            }


            public Builder SetVisible(bool isVisible)
            {
                this.isVisible = isVisible;
                return this;
            }


            public Builder SetOpen(bool isOpen)
            {
                this.isOpen = isOpen;
                return this;
            }

            public Builder SetTtl(int ttl)
            {
                this.ttl = ttl;
                return this;
            }

            public RoomOption Build()
            {
                return new RoomOption(this);
            }
        }
    }
}